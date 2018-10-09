namespace ChickNet.UnitTests.GateTests
{
    public class MockGateBuilder
    {
        private bool _isOpen;
        private int _gateNr;
        private bool _isClosed;

        public MockGateState Build()
        {
            return
                new MockGateState
                {
                    IsOpen = _isOpen,
                    IsClosed = _isClosed,
                    GateNr = _gateNr,
                };
        }

        public MockGateBuilder WithGateOpen()
        {
            _isOpen = true;
            _isClosed = false;

            return this;
        }

        public MockGateBuilder WithGateNr(int nr)
        {
            _gateNr = nr;

            return this;
        }

        public MockGateBuilder WithGateClosed()
        {
            _isOpen = false;
            _isClosed = true;

            return this;
        }
    }
}