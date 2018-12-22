﻿using System;
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
        public int CurrentDutyCyclePercent => (int)Math.Round(_wrappedPin.GetActiveDutyCyclePercentage(), 0);

        /// <inheritdoc />
        public void SetActiveDutyCyclePercent(int newDutyCycle)
        {
            _wrappedPin.SetActiveDutyCyclePercentage(newDutyCycle);
        }

        #endregion
    }
}
