using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Pwm;

namespace ChickNet.Pwm
{
    public class PwmPinWrapper : IPwmPin
    {
        private readonly PwmPin _wrappedPin;

        public PwmPinWrapper(PwmPin wrappedPin)
        {
            _wrappedPin = wrappedPin;
        }

        #region Implementation of IPwmPin

        /// <inheritdoc />
        public int CurrentDutyCyclePercent { get; }

        /// <inheritdoc />
        public void SetActiveDutyCyclePercent(int newDutyCycle)
        {
            _wrappedPin.SetActiveDutyCyclePercentage(newDutyCycle);
        }

        #endregion
    }
}
