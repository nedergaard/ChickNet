namespace ChickNet.Gate
{
    public class GateController
    {
        private readonly ISelector _selector;
        private readonly IPwmController _pwmController;
        private readonly IGateStates _mockGateStates;

        public GateController(ISelector selector, IPwmController pwmController, IGateStates mockGateStates)
        {
            _selector = selector;
            _pwmController = pwmController;
            _mockGateStates = mockGateStates;
        }

        public void OpenGate(int gateNrToOpen)
        {
            _selector.Select(9);
            _pwmController.ChangeDutyCycleTo(50);
        }
    }
}
