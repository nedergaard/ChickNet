using System;
using System.Threading.Tasks;

namespace ChickNet.Pwm
{
    public static class PwmPinExtensions
    {
        public static Task Stop(this IPwmPin pwmPin, int stepsPerChange = 3, int delayPerStepMs = 40)
        {
            return pwmPin.ChangeDutyCycleInStepsAsync(0, stepsPerChange, delayPerStepMs);
        }

        public static Task ChangeDutyCycleInStepsAsync(this IPwmPin pwmPin, int targetDutyCycle,
            int stepsPerChange = 6, int delayPerStepMs = 80)
        {
            var dutyCycleChange = targetDutyCycle - pwmPin.CurrentDutyCycle;
            
            // Adjust stepsPerChange if each step would be less than 1
            stepsPerChange = Math.Min(stepsPerChange, Math.Abs(dutyCycleChange));

            if (stepsPerChange == 0)
            {
                return Task.CompletedTask;
            }

            var fullStep = dutyCycleChange / stepsPerChange;
            return Task.Run(
                async () =>
                {
                    do
                    {
                        pwmPin.SetActiveDutyCycle(pwmPin.CurrentDutyCycle + fullStep);
                        await Task.Delay(delayPerStepMs);

                    } while (Math.Abs(pwmPin.CurrentDutyCycle - targetDutyCycle) > Math.Abs(fullStep));
                    pwmPin.SetActiveDutyCycle(targetDutyCycle);
                });
        }
    }
}