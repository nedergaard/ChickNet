using ChickNetWeb.Selection;
using System.Device.Gpio;

namespace ChickNetWeb.Gate
{
    public class GateState : IGateState
    {
        private readonly IPin _closedSwitchPin;
        private readonly IPin _openSwitchPin;

        public GateState(IPin closedSwitchPin, IPin openSwitchPin)
        {
            _closedSwitchPin = closedSwitchPin;
            _openSwitchPin = openSwitchPin;
        }

        #region Implementation of IGateState

        /// <inheritdoc />
        public bool IsOpen => 
            _closedSwitchPin.Read() == PinValue.High 
            && _openSwitchPin.Read() == PinValue.High;

        /// <inheritdoc />
        public bool IsClosed =>
            _closedSwitchPin.Read() == PinValue.Low 
            && _openSwitchPin.Read() == PinValue.Low;

        #endregion
    }
}