using System.Linq;
using ChickNet.Gate;
using Xunit;

namespace ChickNet.UnitTests.GateTests
{
    public class GateControllerTests
    {
        // [UnitOfWork_StateUnderTest_ExpectedBehavior]
        [Fact]
        public void OpenGate_GateClosed_SelectsGateBeforeActivatingMotor()
        {
            // Arrange
            const int otherGateNr = 2;
            const int gateNrToOpen = 3;

            var mockSelector =
                new MockSelectorBuilder()
                    .WithSelected(otherGateNr)
                    .Build();

            var mockPwmController = new MockPwnController();
                    

            var mockGateStates =
                new MockGateStatesBuilder()
                    .WithGate(atNr: 2)
                    .WithClosedGate(atNr: 3)
                    .Build();
            
            var dut = new GateController(mockSelector, mockPwmController, mockGateStates);
            
            // Act
            dut.OpenGate(gateNrToOpen);

            // Assert
            Assert.True(mockSelector.SelectednrLastChangedTick 
                <= mockPwmController
                    .DutycycleHistory
                        .Where(dce => dce.DutyCycle > 0) 
                        .Min(dce => dce.Tick));
        }
    }
}
