using System.Collections.Generic;

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
            _gates.Add(new MockGateBuilder().Build());

            return this;
        }

        public MockGateStatesBuilder WithClosedGate(int atNr)
        {
            _gates.Add(
                new MockGateBuilder()
                    .WithGateOpen()
                    .Build());

            return this;
        }

        public MockGateStates Build()
        {
            return new MockGateStates(_gates);            
        }
    }
}