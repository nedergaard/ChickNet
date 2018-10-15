using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Gpio;
using ChickNet.Gate;

namespace ChickNet.Selection
{
    public class Selector : ISelector
    {
        private List<IPin> _pins;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pins">List of pins ordered from least significat to most.</param>
        public Selector(IEnumerable<IPin> pins)
        {
            _pins = pins.ToList();
        }

        #region Implementation of ISelector

        /// <inheritdoc />
        public int Selected { get; private set; }

        /// <inheritdoc />
        public void Select(int itemNr)
        {
            for (var pinIndex = 0; pinIndex < _pins.Count; pinIndex++)
            {
                _pins[pinIndex].Write((1 << pinIndex & itemNr) > 0 ? GpioPinValue.High : GpioPinValue.Low);
            }

            Selected = itemNr;
        }

        #endregion
    }

    public interface IPin
    {
        GpioPinValue Read();

        void Write(GpioPinValue newValue);
    }
}
