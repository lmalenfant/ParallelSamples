﻿#if BENCHMARK
using BenchmarkDotNet.Attributes;
#endif
using ParallelSamples.DataStructures;
using System.Threading;

namespace ParallelSamples.MonteCarlo
{
    public class MonteCarloSimulation
    {
        public SimulationInput Input { get; set; }

        public SimulationOutput Results { get; private set; }

        public int Rng { get; set; }

        public MonteCarloSimulation() : this(new SimulationInput(), 4) { }

        public MonteCarloSimulation(SimulationInput input) : this(input, 4) { }

        public MonteCarloSimulation(SimulationInput input, int rng)
        {
            Input = input;
            Rng = rng;
        }

#if BENCHMARK
        [Benchmark]
#endif
        public SimulationOutput Run()
        {
            Results = new SimulationOutput(Input);
            Thread.Sleep(100);
            return Results;
        }

    }
}