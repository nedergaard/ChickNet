using System.Device.Gpio;

namespace ChickNetWeb.Hardware
{
    public class GpioPinWrapper : IPin
    {
        private readonly int _pinNr;
        private PinMode _pinMode;
        private GpioController _gpioController;

        public GpioPinWrapper(int pinNr, PinMode pinMode, GpioController gpioController)
        {
            _pinNr = pinNr;
            _pinMode = pinMode;
            _gpioController = gpioController;

            _gpioController.OpenPin(_pinNr, pinMode);
        }

        #region Implementation of IPin

        /// <inheritdoc />
        public PinValue Read()
        {
            return _gpioController.Read(_pinNr);
        }

        /// <inheritdoc />
        public void Write(PinValue newValue)
        {
            _gpioController.Write(_pinNr, newValue);
        }

        #endregion
    }
}
