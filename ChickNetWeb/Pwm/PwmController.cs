using System;
using System.Threading.Tasks;

namespace ChickNetWeb.Pwm
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
            if (value == _activePin)
            {
                return;
            }

            await inactivePin.ChangeDutyCycleInStepsAsync(0);

            _activePin = value;
        }

        #region Implementation of IPwmController

        /// <inheritdoc />
        public Direction Direction { get; private set; }

        /// <inheritdoc />
        public async Task SetDirectionAsync(Direction newDirection)
        {
            await SetActivePinAsync(
                newDirection == Direction.Forward
                    ? _forwardPwmPin
                    : _backwardPwmPin);

            Direction = newDirection;

            await ChangeDutyCyclePercentAsync(DutyCyclePercent);
        }

        /// <inheritdoc />
        public int DutyCyclePercent { get; private set; }

        /// <inheritdoc />
        public async Task ChangeDutyCyclePercentAsync(int percent)
        {
            var targetDutyCycle = Math.Min(100, Math.Max(0, percent));

            await _activePin.ChangeDutyCycleInStepsAsync(targetDutyCycle, _stepsPerChange, 80);

            DutyCyclePercent = percent;
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
        int CurrentDutyCyclePercent { get; }

        void SetActiveDutyCyclePercent(int newDutyCycle);
    }
}