using System;
using System.Device.Pwm;

namespace ChickNetWeb.Pwm
{
    public class PwmChannelWrapper : IPwmPin
    {
        private readonly PwmChannel _wrappedChannel;

        public PwmChannelWrapper(PwmChannel pwmChannel)
        {
            _wrappedChannel = pwmChannel;

            _wrappedChannel.DutyCycle = 0;
            _wrappedChannel.Start();
        }

        public int CurrentDutyCyclePercent => (int)Math.Round(_wrappedChannel.DutyCycle * 100, 0);


        public void SetActiveDutyCyclePercent(int newDutyCycle)
        {
            double newDutyCycleFraction = Math.Round(newDutyCycle / 100D, 2);
            _wrappedChannel.DutyCycle = newDutyCycleFraction;
        }
    }
}
