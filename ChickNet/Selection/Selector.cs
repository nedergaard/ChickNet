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
        public int Selected { get; }

        /// <inheritdoc />
        public void Select(int itemNr)
        {
            
        }

        #endregion
    }

    public interface IPin
    {
        GpioPinValue Read();

        void Write(GpioPinValue newValue);
    }
}
