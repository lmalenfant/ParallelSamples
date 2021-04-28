using System;
using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using ParallelSamples.MonteCarlo;

namespace ParallelSamples.Benchmark
{
    public static class Program
    {
        /// <summary>
        /// Run the Benchmarks for the attached application
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // We need to setup a manual configuration so the Benchmark will run in
            // a different build configuration, in this case Benchmark 
            var config = new ManualConfig()
                .AddJob(new Job("Benchmark").WithCustomBuildConfiguration("Benchmark"))
                .AddLogger(ConsoleLogger.Default)
                .AddColumn(TargetMethodColumn.Method,
                    StatisticColumn.Mean,
                    StatisticColumn.Error,
                    StatisticColumn.StdDev,
                    StatisticColumn.Median)
                .AddExporter(CsvExporter.Default, HtmlExporter.Default, MarkdownExporter.GitHub)
                .AddAnalyser(EnvironmentAnalyser.Default);
            config.UnionRule = ConfigUnionRule.Union; 
            var summary = BenchmarkRunner.Run<MonteCarloSimulation>(config);
            Console.WriteLine(summary);
            summary = BenchmarkRunner.Run<ParallelMonteCarloSimulation>(config);
            Console.WriteLine(summary);
        }
    }
}
