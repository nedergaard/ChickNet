using Windows.Devices.Gpio;
using ChickNet.Selection;

namespace ChickNet.Gate
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
            _closedSwitchPin.Read() == GpioPinValue.High 
            && _openSwitchPin.Read() == GpioPinValue.High;

        /// <inheritdoc />
        public bool IsClosed =>
            _closedSwitchPin.Read() == GpioPinValue.Low 
            && _openSwitchPin.Read() == GpioPinValue.Low;

        #endregion
    }
}