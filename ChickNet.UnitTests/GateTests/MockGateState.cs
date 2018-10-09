using ChickNet.Gate;

namespace ChickNet.UnitTests.GateTests
{
    public class MockGateState : IGateState
    {
        public int GateNr { get; set; }

        #region Implementation of IGateState

        /// <inheritdoc />
        public bool IsOpen { get; set; }

        /// <inheritdoc />
        public bool IsClosed { get; set; }

        #endregion
    }
}