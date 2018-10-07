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

            var fixture = new GateControllerFixture();
            var dut =
                fixture
                    .WithClosedGate(atNr: gateNrToOpen)
                    .WithClosedGate(atNr: otherGateNr)
                    .WithSelectedGate(otherGateNr)
                    .NewDut();

            // Act
            dut.OpenGate(gateNrToOpen);

            // Assert
            Assert.True(fixture.SelectednrLastChangedTick <= fixture.FirstNonZeroDutycycleChangeTick);
        }
    }
}
