using ParallelSamples.IO;

namespace ParallelSamples.DataStructures
{
    public class SimulationInput
    {
        public long N { get; set; }
        public int SimulationIndex { get; set; }

        /// <summary>
        /// Default Constructor for SimulationInput
        /// </summary>
        public SimulationInput() : this(1000000) { }

        /// <summary>
        /// Constructor for SimulationInput
        /// </summary>
        public SimulationInput(int n)
        {
            N = n;
        }

        /// <summary>
        /// Method to duplicate SimulationInput
        /// </summary>
        /// <returns>SimulationInput</returns>
        public SimulationInput Duplicate()
        {
            return this.DuplicateObjectViaSerialization();
        }
    }
}