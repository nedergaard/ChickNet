namespace ChickNet.UnitTests.GateTests
{
    public class MockGateBuilder
    {
        private bool _isOpen;

        public MockGateState Build()
        {
            return new MockGateState();
        }

        public MockGateBuilder WithGateOpen()
        {
            _isOpen = true;

            return this;
        }
    }
}