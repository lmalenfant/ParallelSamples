using System;
using System.IO;
using ParallelSamples.DataStructures;
using ParallelSamples.MonteCarlo;

namespace ParallelSamples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var parallelMonteCarlo = new ParallelMonteCarloSimulation(new SimulationInput(10000000), 4);
            Console.WriteLine("Run MC with partitions (Parallel.ForEach loop):");
            parallelMonteCarlo.RunMonteCarloWithPartitions();
            Console.WriteLine("Run MC (Parallel.For loop):");
            parallelMonteCarlo.RunMonteCarlo();
            var mc = new MonteCarloSimulation();
            var simulationOutput = mc.Run();
            Console.WriteLine($"Number of photons is: {simulationOutput.Input.N}");
        }
    }
}
