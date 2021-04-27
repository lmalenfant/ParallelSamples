using NUnit.Framework;
using ParallelSamples.DataStructures;
using ParallelSamples.IO;

namespace ParallelSamples.Test.IO
{
    internal class FileIoTests
    {
        [Test]
        public void Test_duplicate_object()
        {
            var input = new SimulationInput(100000)
            {
                SimulationIndex = 10
            };
            var inputCopy = input.DuplicateObjectViaSerialization<SimulationInput>();
            Assert.AreEqual(input.N, inputCopy.N);
            Assert.AreEqual(input.SimulationIndex, inputCopy.SimulationIndex);
            input.N = 100;
            input.SimulationIndex = 0;
            Assert.AreNotEqual(input.N, inputCopy.N);
            Assert.AreNotEqual(input.SimulationIndex, inputCopy.SimulationIndex);
        }
    }
}
