using System.Threading.Tasks;
using Xunit;

namespace ChickNet.UnitTests.GateTests
{
    public class GateControllerTests
    {
        // [UnitOfWork_StateUnderTest_ExpectedBehavior]
        [Fact]
        public async Task OpenGate_GateClosed_SelectsGateBeforeActivatingMotorAsync()
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
            await dut.OpenGateAsync(gateNrToOpen);

            // Assert
            Assert.True(fixture.SelectednrLastChangedTick <= fixture.FirstNonZeroDutycycleChangeTick);
        }
    }

    // I am not writing more of these until I get Rx
}
