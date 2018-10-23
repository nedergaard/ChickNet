using System;
using System.Threading.Tasks;
using ChickNet.Gate;

namespace ChickNet.Pwm
{
    public class PwmController : IPwmController
    {
        private readonly IPwmPin _forwardPwmPin;
        private readonly IPwmPin _backwardPwmPin;
        private readonly int _stepsPerChange;

        public PwmController(IPwmPin forwardPwmPin, IPwmPin backwardPwmPin, int stepsPerChange)
        {
            _forwardPwmPin = forwardPwmPin;
            _backwardPwmPin = backwardPwmPin;

            _activePin = forwardPwmPin;

            _stepsPerChange = stepsPerChange;

            Direction = Direction.Forward;
        }

        private IPwmPin _activePin;

        private async Task SetActivePinAsync(IPwmPin value)
        {
            var inactivePin = value == _forwardPwmPin ? _backwardPwmPin : _forwardPwmPin;
            inactivePin.SetActiveDutyCycle(0);

            if (value == _activePin)
            {
                return;
            }

            await StopAsync();

            _activePin = value;
        }

        #region Implementation of IPwmController

        /// <inheritdoc />
        public Direction Direction { get; set; }

        /// <inheritdoc />
        public int DutyCyclePercent => _activePin.CurrentDutyCycle;

        /// <inheritdoc />
        public async Task ChangeDutyCyclePercentAsync(int percent)
        {
            var targetDutyCycle =(int)Math.Round(Math.Min(100, Math.Max(0, percent)) * 2.55);

            await _activePin.ChangeDutyCycleInStepsAsync(targetDutyCycle, _stepsPerChange, 80);
        }

        /// <inheritdoc />
        public async Task StopAsync()
        {
            // NOTEST 
            await _activePin.Stop(Math.Min(2, _stepsPerChange), Math.Min(40, 80));
        }

        #endregion
    }

    public interface IPwmPin
    {
        int CurrentDutyCycle { get; }

        void SetActiveDutyCycle(int newDutyCycle);
    }
}