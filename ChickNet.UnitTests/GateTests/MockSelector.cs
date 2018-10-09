using System;
using ChickNet.Gate;

namespace ChickNet.UnitTests.GateTests
{
    public class MockSelector : ISelector
    {
        public int SelectedNr { get; set; }
        public long? SelectednrLastChangedTick { get; protected set; }

        #region Implementation of ISelector

        /// <inheritdoc />
        public int Selected { get; set; }

        /// <inheritdoc />
        public void Select(int itemNr)
        {
            SelectedNr = itemNr;
            SelectednrLastChangedTick = DateTime.Now.Ticks;
        }

        #endregion
    }
}
