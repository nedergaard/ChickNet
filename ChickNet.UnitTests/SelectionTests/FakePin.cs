using ChickNetWeb.Selection;
using System.Device.Gpio;

namespace ChickNet.UnitTests.SelectionTests
{
    public class FakePin : IPin
    {
        public PinValue CurrrentPinValue { get; set; }

        #region Implementation of IPin

        /// <inheritdoc />
        public PinValue Read()
        {
            return CurrrentPinValue;
        }

        /// <inheritdoc />
        public void Write(PinValue newValue)
        {
            CurrrentPinValue = newValue;
        }

        #endregion
    }
}
