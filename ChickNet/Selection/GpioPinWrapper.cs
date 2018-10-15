using Windows.Devices.Gpio;

namespace ChickNet.Selection
{
    public class GpioPinWrapper : IPin
    {
        private readonly GpioPin _wrappedPin;

        public GpioPinWrapper(GpioPin wrappedPin)
        {
            _wrappedPin = wrappedPin;
        }

        #region Implementation of IPin

        /// <inheritdoc />
        public GpioPinValue Read()
        {
            return _wrappedPin.Read();
        }

        /// <inheritdoc />
        public void Write(GpioPinValue newValue)
        {
            _wrappedPin.Write(newValue);
        }

        #endregion
    }
}
