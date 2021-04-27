using System;
using BenchmarkDotNet.Running;
using ParallelSamples.MonteCarlo;

namespace ParallelSamples.Benchmark
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<MonteCarloSimulation>();
            Console.WriteLine(summary);
            summary = BenchmarkRunner.Run<ParallelMonteCarloSimulation>();
            Console.WriteLine(summary);
        }
    }
}
