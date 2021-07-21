using ChickNetWeb.Pwm;
using ChickNetWeb.Selection;
using System;
using System.Threading.Tasks;

namespace ChickNetWeb.Gate
{
    public class GateController
    {
        private readonly ISelector _selector;
        private readonly IPwmController _pwmController;
        private readonly IGateStates _gateStates;

        public GateController(ISelector selector, IPwmController pwmController, IGateStates gateStates)
        {
            _selector = selector;
            _pwmController = pwmController;
            _gateStates = gateStates;
        }

        public async Task OpenGateAsync(int gateNrToOpen)
        {
            _selector.Select(gateNrToOpen);

            bool IsOpen()
            {
                return _gateStates.GetStateOf(gateNrToOpen).IsOpen;
            }

            if (IsOpen())
            {
                return;
            }

            await _pwmController.SetDirectionAsync(Direction.Forward);
            // Accelerate motor
            await _pwmController.ChangeDutyCyclePercentAsync(100);

            var timeOutTime = DateTime.Now.AddSeconds(60);
            // Wait for gate to be fully open
            while (!IsOpen() && DateTime.Now < timeOutTime)
            {
                await Task.Delay(200);
            }

            await _pwmController.ChangeDutyCyclePercentAsync(0);
        }

        public async Task CloseGateAsync(int gateNrToClose)
        {
            _selector.Select(gateNrToClose);

            bool IsClosed()
            {
                return _gateStates.GetStateOf(gateNrToClose).IsClosed;
            }

            if (IsClosed())
            {
                return;
            }

            await _pwmController.SetDirectionAsync(Direction.Backward);
            await _pwmController.ChangeDutyCyclePercentAsync(100);

            var timeOutTime = DateTime.Now.AddSeconds(60);
            // Wait for gate to be fully open
            while (!IsClosed() && DateTime.Now < timeOutTime)
            {
                await Task.Delay(200);
            }

            await _pwmController.ChangeDutyCyclePercentAsync(0);
        }
    }
}
