using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using ParallelSamples.DataStructures;

namespace ParallelSamples.MonteCarlo
{
    public class ParallelMonteCarloSimulation
    {
        public int NumberOfCpus { get; set; }

        public SimulationInput Input { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ParallelMonteCarloSimulation(): this(new SimulationInput(), 2) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="input">SimulationInput</param>
        /// <param name="numberOfCpus">Number of CPUs</param>
        public ParallelMonteCarloSimulation(SimulationInput input, int numberOfCpus)
        {
            Input = input;
            NumberOfCpus = numberOfCpus;
        }

        [Benchmark]
        public void RunMonteCarloWithPartitions()
        {
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = NumberOfCpus
            };
            var simulationOutputs = new ConcurrentBag<SimulationOutput>();

            var photonsPerCpu = Input.N / NumberOfCpus;
            var partition = Partitioner.Create(0, Input.N, photonsPerCpu);
            Parallel.ForEach<Tuple<long, long>, MonteCarloSimulation>(partition, parallelOptions, () => new MonteCarloSimulation(Input),
                (tSource, parallelLoopState, partitionIndex, monteCarloSimulation) =>
                {
                    try
                    {
                        // create a random number
                        monteCarloSimulation.Rng = new Random(5).Next();
                        monteCarloSimulation.Input.SimulationIndex = (int)partitionIndex;
                        var (item1, item2) = tSource;
                        Console.WriteLine($"Partition({partitionIndex}) is processing {item1} to {item2} - id:{Task.CurrentId}");
                        monteCarloSimulation.Run();
                        return monteCarloSimulation;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }, (x) =>
                {
                    try
                    {
                        simulationOutputs.Add(x.Results);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                });
            Console.WriteLine(simulationOutputs.Count);
            Console.WriteLine(Input.SimulationIndex);
        }

        [Benchmark]
        public void RunMonteCarlo()
        {
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = NumberOfCpus
            };
            var simulationOutputs = new ConcurrentBag<SimulationOutput>();
            var simulationInputs = new ConcurrentQueue<SimulationInput>();
            simulationInputs.Enqueue(Input);
            Parallel.For<MonteCarloSimulation>(0, parallelOptions.MaxDegreeOfParallelism, parallelOptions, () => simulationInputs.TryPeek(out var input) ? new MonteCarloSimulation(input) : new MonteCarloSimulation(),
                (index, parallelLoopState, monteCarloSimulation) =>
                {
                    try
                    {
                        // create a random number
                        monteCarloSimulation.Rng = new Random(5).Next();
                        monteCarloSimulation.Input.SimulationIndex = index;
                        Console.WriteLine($"Index({index}) is processing - id:{Task.CurrentId}");
                        monteCarloSimulation.Run();
                        return monteCarloSimulation;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }, (x) =>
                {
                    try
                    {
                        simulationOutputs.Add(x.Results);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                });
            Console.WriteLine(Input.SimulationIndex);
        }
    }
}