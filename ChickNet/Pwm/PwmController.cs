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

            _stepsPerChange = stepsPerChange;

            Direction = Direction.Forward;
            _dutyCycle = 0;
        }

        private IPwmPin _activePin;

        private async Task SetActivePinAsync(IPwmPin value)
        {
            var inactivePin = value == _forwardPwmPin ? _backwardPwmPin : _forwardPwmPin;
            inactivePin.SetActiveDutyCyclePercentage(0);

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

        private int _dutyCycle;

        /// <inheritdoc />
        public int DutyCycle
        {
            get => _dutyCycle;
            private set
            {
                _dutyCycle = value;
                _activePin.SetActiveDutyCyclePercentage(_dutyCycle);
            }
        }

        /// <inheritdoc />
        public async Task ChangeDutyCycleAsync(int percent)
        {
            var targetDutyCycle = Math.Min(100, Math.Max(0, percent)) * 255;

            await InternalChangeDutyCycleAsync(targetDutyCycle, _stepsPerChange, 80);
        }

        /// <inheritdoc />
        public async Task StopAsync()
        {
            await InternalChangeDutyCycleAsync(0, Math.Min(2, _stepsPerChange), Math.Min(40, 80));
        }

        private async Task InternalChangeDutyCycleAsync(int targetDutyCycle, int stepsPerChange, int delayPerStepMS)
        {
            var step = (targetDutyCycle - DutyCycle) / stepsPerChange;
            do
            {
                DutyCycle += step;
                await Task.Delay(delayPerStepMS);

            } while (Math.Abs(DutyCycle - targetDutyCycle) > step);

            DutyCycle = targetDutyCycle;
        }

        #endregion
    }

    public interface IPwmPin
    {
        int CurrentDutyCycle { get; }

        void SetActiveDutyCyclePercentage(int newDutyCyclePct);
    }
}