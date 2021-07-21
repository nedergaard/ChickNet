using System.Collections.Generic;
using System.Linq;
using ChickNetWeb.Gate;
using Moq;

namespace ChickNet.UnitTests.GateTests
{
    public class MockGateStatesBuilder
    {
        private readonly List<MockGateState> _gates;

        public MockGateStatesBuilder()
        {
            _gates = new List<MockGateState>();
        }

        public MockGateStatesBuilder WithGate(int atNr)
        {
            WithClosedGate(atNr);

            return this;
        }

        public MockGateStatesBuilder WithClosedGate(int atNr)
        {
            _gates.Add(
                new MockGateBuilder()
                    .WithGateNr(atNr)
                    .WithGateClosed()
                    .Build());

            return this;
        }

        public Mock<IGateStates> Build()
        {
            var result = new Mock<IGateStates>();
            result
                .Setup(m => m.GetStateOf(It.IsAny<int>()))
                .Returns((int nr) => _gates.FirstOrDefault(gate => gate.GateNr == nr));
            return result;
        }
    }
}