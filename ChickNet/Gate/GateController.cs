using System;
using System.Threading.Tasks;

namespace ChickNet.Gate
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

            _pwmController.Direction = Direction.Forward;
            // Accelerate motor
            var percent = 10;
            do
            {
                percent += 10;
                await Task.Delay(80);

            } while (percent < 100 || IsOpen());

            var timeOutTime = DateTime.Now.AddSeconds(20);
            // Wait for gate to be fully open
            while (!IsOpen() && DateTime.Now < timeOutTime)
            {
                await Task.Delay(200);
            }

            _pwmController.ChangeDutyCycleTo(0);
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

            _pwmController.Direction = Direction.Backward;
            // Accelerate motor
            var percent = 10;
            do
            {
                percent += 10;
                await Task.Delay(80);

            } while (percent < 100 || IsClosed());

            var timeOutTime = DateTime.Now.AddSeconds(20);
            // Wait for gate to be fully open
            while (!IsClosed() && DateTime.Now < timeOutTime)
            {
                await Task.Delay(200);
            }

            _pwmController.ChangeDutyCycleTo(0);
        }
    }
}
