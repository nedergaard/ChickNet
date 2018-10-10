using Windows.Devices.Gpio;
using ChickNet.Selection;

namespace ChickNet.UnitTests.SelectionTests
{
    public class FakePin : IPin
    {
        public GpioPinValue CurrrentPinValue { get; set; }

        #region Implementation of IPin

        /// <inheritdoc />
        public GpioPinValue Read()
        {
            return CurrrentPinValue;
        }

        /// <inheritdoc />
        public void Write(GpioPinValue newValue)
        {
            CurrrentPinValue = newValue;
        }

        #endregion
    }
}
