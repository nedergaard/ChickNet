using System;
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
        public int CurrentDutyCyclePercent => (int)Math.Round(_wrappedPin.GetActiveDutyCyclePercentage() * 100, 0);

        /// <inheritdoc />
        public void SetActiveDutyCyclePercent(int newDutyCycle)
        {
            double newDutyCyclePercent = Math.Round(newDutyCycle / 100D, 2);
            _wrappedPin.SetActiveDutyCyclePercentage(newDutyCyclePercent);

            if (!_wrappedPin.IsStarted)
            {
                _wrappedPin.Start();
            }
            if (_wrappedPin.IsStarted && newDutyCyclePercent < 0.01)
            {
                _wrappedPin.Stop();
            }
        }

        #endregion
    }
}
