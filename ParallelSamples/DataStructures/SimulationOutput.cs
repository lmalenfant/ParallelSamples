namespace ParallelSamples.DataStructures
{
    public class SimulationOutput
    {
        public SimulationInput Input { get; set; }

        public SimulationOutput(SimulationInput input)
        {
            Input = input;
        }
        public SimulationOutput() : this(new SimulationInput()) { }
    }
}