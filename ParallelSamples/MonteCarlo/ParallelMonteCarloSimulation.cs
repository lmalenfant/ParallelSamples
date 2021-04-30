#if BENCHMARK
using BenchmarkDotNet.Attributes;
#endif
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
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

#if BENCHMARK
        [Benchmark]
#endif
        public void RunMonteCarloWithPartitions()
        {
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = NumberOfCpus
            };
            var simulationOutputs = new ConcurrentBag<SimulationOutput>();

            var photonsPerCpu = Input.N / NumberOfCpus;
            var partition = Partitioner.Create(0, Input.N, photonsPerCpu);
            Parallel.ForEach<Tuple<long, long>, MonteCarloSimulation>(partition, parallelOptions, () => new MonteCarloSimulation(Input.Duplicate()),
                (tSource, parallelLoopState, partitionIndex, monteCarloSimulation) =>
                {
                    try
                    {
                        // create a random number
                        monteCarloSimulation.Rng = new Random(5).Next();
                        monteCarloSimulation.Input.SimulationIndex = (int)partitionIndex;
                        var (item1, item2) = tSource;
                        Console.WriteLine($"Partition({partitionIndex}) is processing {item1} to {item2} - id:{Task.CurrentId} Photons: {monteCarloSimulation.Input.N}");
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
            Console.WriteLine($"Values in SimulationOutput: {simulationOutputs.Count}");
            Console.WriteLine($"Original index: {Input.SimulationIndex}");
        }

#if BENCHMARK
        [Benchmark]
#endif
        public void RunMonteCarlo()
        {
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = NumberOfCpus
            };
            var simulationOutputs = new ConcurrentBag<SimulationOutput>();

            Parallel.For<MonteCarloSimulation>(0, parallelOptions.MaxDegreeOfParallelism, parallelOptions, () => new MonteCarloSimulation(Input.Duplicate(), 10),
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
            Console.WriteLine($"Values in SimulationOutput: {simulationOutputs.Count}");
            Console.WriteLine($"Original index: {Input.SimulationIndex}");
        }
    }
}