﻿using flow_genetic_algorithm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAF;
using GAF.Operators;
using GAF.Extensions;
using System.Diagnostics;

namespace flow_genetic_algorithm
{
    public class FlowGeneticAlgorithm
    {
        #region Data Members

        private Models.Task[] tasks;
        private User[] users;
        private Dictionary<string, Calendar> calendars;
        private Board board;
        private Stopwatch watch;

        #endregion

        #region Ctor

        public FlowGeneticAlgorithm(Board board, IEnumerable<Calendar> calendars)
        {
            this.tasks = board.tasks.Where(t => t.status != "done").ToArray();
            this.users = board.boardMembers.ToArray<User>();
            this.calendars = calendars.ToDictionary<Calendar, string>(k => k.uid);
            this.board = board;
            this.watch = new Stopwatch();
        } 
        
        #endregion

        #region Public Methods

        public Board GetBoardSuggestion()
        {
            var population = new Population();

            var randomGen = new Random();

            //create the chromosomes
            for (var p = 0; p < 100; p++)
            {
                var chromosome = new Chromosome();                
                for (var g = 0; g < this.tasks.Length; g++)
                {
                    chromosome.Genes.Add(new Gene(randomGen.Next(0, this.users.Length)));
                }

                chromosome.Genes.ShuffleFast();

                population.Solutions.Add(chromosome);
            }

            //create the elite operator
            var elite = new Elite(5);

            //create the crossover operator
            var crossover = new Crossover(0.8)
            {
                CrossoverType = CrossoverType.SinglePoint
            };

            //create the mutation operator
            var mutate = new SwapMutate(0.02);

            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //hook up to some useful events
            ga.OnGenerationComplete += ga_OnGenerationComplete;
            ga.OnRunComplete += ga_OnRunComplete;

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            this.watch.Start();

            //run the GA
            ga.Run(Terminate);

            var suggestedBoard = createBoardFromChromose(ga.Population.GetTop(1).First());
            var doneTasks = board.tasks.Where(t => t.status.Equals("done"));
            board.tasks = suggestedBoard.tasks;
            board.tasks.AddRange(doneTasks);

            return board;    
        } 

        #endregion

        #region Private Methods

        private void ga_OnRunComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            Console.Write(fittest.Fitness);
        }

        private void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {            
            var fittest = e.Population.GetTop(1)[0];
            Console.Write(string.Format("Generation:{0}, Popolation: {1} ,Fitness: {2}\n", e.Generation, e.Population.Solutions.Count ,fittest.Fitness));
            
        }

        private double CalculateFitness(Chromosome chromosome)
        {
            Board board = createBoardFromChromose(chromosome);

            return calculateBoardFitness(board);
        }

        private double calculateBoardFitness(Board board)
        {            
            var maxTask = board.tasks.Max(t => t.endTime);

            if (maxTask > board.endDate)
                return 0;

            var timeToFinishBoard = (maxTask.ToLocalTime() - board.startDate.ToLocalTime()).TotalHours;

            var fitness = 1 / timeToFinishBoard;

            if (fitness > 1)
                fitness = 1;

            if (fitness < 0)
                fitness = 0;

            return fitness;
        }

        private Board createBoardFromChromose(Chromosome chromosome)
        {
            var board = new Board();
            board.startDate = this.board.startDate.ToLocalTime();
            board.endDate = this.board.endDate.ToLocalTime();
            board.tasks = new List<Models.Task>();

            var usersTasks = getTasksDistribution(chromosome);

            foreach (var userTasks in usersTasks)
            {
                var userCalendar = this.calendars.ContainsKey(userTasks.Key.uid) ?
                    (Calendar)this.calendars[userTasks.Key.uid].Clone() : new Calendar();

                foreach (var task in userTasks.Value)
                {
                    placeTaskInBoard(board, userCalendar, task, userTasks.Key);
                }

            }

            return board;
        }

        private void placeTaskInBoard(Board board, Calendar userCalendar, Models.Task task, User user)
        {
            var relevantEvents = getRelevantEvents(board, userCalendar);

            Event newEvent = new Event()
            {
                title = task.title,
                eventId = System.Guid.NewGuid().ToString(),
                startDate = board.startDate.ToLocalTime(),
                endDate = board.startDate.ToLocalTime().AddHours(task.remainingTime),
            };

            foreach (var userEvent in relevantEvents.OrderBy(e => e.startDate))
            {
                if (newEvent.doesEventsOverlapping(userEvent))
                {
                    newEvent.startDate = userEvent.endDate.ToLocalTime();
                    newEvent.endDate = userEvent.endDate.ToLocalTime().AddHours(task.remainingTime);
                }
                else
                {
                    break;
                }
            }

            userCalendar.events.Add(newEvent);

            board.tasks.Add(new Models.Task()
            {
                boardId = task.boardId,
                boardName = task.boardName,
                owner = user,
                overallTime = task.overallTime,
                remainingTime = task.remainingTime,
                status = task.status,
                taskId = task.taskId,
                title = task.title,
                startTime = newEvent.startDate.ToLocalTime(),
                endTime = newEvent.endDate.ToLocalTime()
            });
        }

        private IEnumerable<Event> getRelevantEvents(Board board, Calendar userCalendar)
        {
            var userEvents = userCalendar.events.Where(e => e.doesEventsOverlapping(new Event() { startDate = board.startDate, endDate = board.endDate }));

            return userEvents;
        }

        private Dictionary<User, List<Models.Task>> getTasksDistribution(Chromosome chromosome)
        {
            var usersTasks = new Dictionary<User, List<Models.Task>>();

            for (int taskIndex = 0; taskIndex < chromosome.Genes.Count; taskIndex++)
            {
                var userIndex = chromosome.Genes[taskIndex];
                var user = this.users[(int)userIndex.ObjectValue];

                if (usersTasks.ContainsKey(user))
                {
                    usersTasks[user].Add(this.tasks[taskIndex]);
                }
                else
                {
                    usersTasks[user] = new List<Models.Task>()
                    {
                        this.tasks[taskIndex]
                    };
                }
            }

            return usersTasks;
        }

        private bool Terminate(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 100 || this.watch.Elapsed.TotalSeconds > 30;
        } 
        
        #endregion
    }
}
