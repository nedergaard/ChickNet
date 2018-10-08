using System.Collections.Generic;
using System.Linq;
using ChickNet.Gate;

namespace ChickNet.UnitTests.GateTests
{
    public class MockGateStates : IGateStates
    {
        private readonly IList<MockGateState> _mockGateStates;

        public MockGateStates(IEnumerable<MockGateState> mockGateStates)
        {
            _mockGateStates = mockGateStates.ToList();
        }

        #region Implementation of IGateStates

        /// <inheritdoc />
        public IGateState GetStateOf(int gateNr)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}