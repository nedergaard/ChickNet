using System.Linq;
using ChickNet.Gate;
using Moq;

namespace ChickNet.UnitTests.GateTests
{
    public class GateControllerFixture
    {
        private Mock<ISelector> _selector;
        private MockPwnController _pwmController;
        private MockGateStates _gateStates;

        public MockSelectorBuilder SelectorBuilder { get; }
        public MockPwnController MockPwmController { get; }
        public MockGateStatesBuilder GateStatesBuilder { get; }

        /// <summary>
        /// Tick where a gate was last selected
        /// </summary>
        public long? SelectednrLastChangedTick { get; private set; }

        /// <summary>
        /// Tick where the PwmController was first told to start.
        /// </summary>
        public long? FirstNonZeroDutycycleChangeTick
        {
            get
            {
                return 
                    _pwmController
                        .DutycycleHistory
                        .Where(dce => dce.DutyCycle > 0)
                        .Min(dce => dce.Tick);
            }
        }
            
        public GateControllerFixture()
        {
            SelectorBuilder = new MockSelectorBuilder();
            MockPwmController = new MockPwnController();
            GateStatesBuilder = new MockGateStatesBuilder();
        }

        public GateControllerFixture WithClosedGate(int atNr)
        {
            GateStatesBuilder.WithClosedGate(atNr);

            return this;
        }

        public GateControllerFixture WithSelectedGate(int gateNr)
        {
            SelectorBuilder.WithSelected(gateNr);

            return this;
        }

        public GateController NewDut()
        {
            _selector = SelectorBuilder.Build();
            _pwmController = MockPwmController;
            _gateStates = GateStatesBuilder.Build();

            return new GateController(_selector.Object, _pwmController, _gateStates);
        }
    }
}