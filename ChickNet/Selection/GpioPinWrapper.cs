using Windows.Devices.Gpio;

namespace ChickNet.Selection
{
    public class GpioPinWrapper : IPin
    {
        private readonly GpioPin _wrappedPin;

        // TODO : Create elegant way to handle drive modes
        public GpioPinWrapper(GpioPin wrappedPin, GpioPinDriveMode gpioPinDriveMode)
        {
            _wrappedPin = wrappedPin;

            _wrappedPin.SetDriveMode(gpioPinDriveMode);
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
