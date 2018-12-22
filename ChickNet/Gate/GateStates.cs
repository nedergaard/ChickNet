using System;
using System.Collections.Generic;
using System.Linq;

namespace ChickNet.Gate
{
    public class GateStates : IGateStates
    {
        private readonly Dictionary<int, IGateState> _gateStateItems;

        public GateStates()
        {
            _gateStateItems = new Dictionary<int, IGateState>();
        }

        public void Add(IGateState gateState, int gateNr)
        {
            if (_gateStateItems.ContainsKey(gateNr))
            {
                throw new ArgumentException($"An item with gate number {gateNr} already exist.");
            }

            _gateStateItems.Add(gateNr, gateState);
        }

        #region Implementation of IGateStates

        /// <inheritdoc />
        public IGateState GetStateOf(int gateNr)
        {
            if (!_gateStateItems.ContainsKey(gateNr))
            {
                throw new KeyNotFoundException($"No item with gate number {gateNr} found.");
            }

            return _gateStateItems[gateNr];
        }

        #endregion
    }
}