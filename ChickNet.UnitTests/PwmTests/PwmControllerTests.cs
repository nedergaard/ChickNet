using System.Threading.Tasks;
using ChickNet.Gate;
using ChickNet.Pwm;
using FluentAssertions;
using Moq;
using Xunit;

namespace ChickNet.UnitTests.PwmTests
{
    public class PwmControllerTests
    {
        // [UnitOfWork_StateUnderTest_ExpectedBehavior]
        [Theory]
        [InlineData(100, 255)]
        [InlineData(0, 0)]
        [InlineData(20, 51)]
        public async Task ChangeDutyCycleAsyncToPercent_AtZero_TranslatesToByte(int percentInput, int expected)
        {
            // Arrange
            var fixture = new PwmControllerFixture();

            var dut = await fixture.CreateDutAsync();

            // Act
            await dut.ChangeDutyCyclePercentAsync(percentInput);

            // Assert
            fixture.ForwardPwmPin.CurrentDutyCycle.Should().Be(expected);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(25)]
        public async Task ChangeDutyCycleAsyncToPercent_AtZero_DutyCyclePercentIsUpdated(int expected)
        {
            // Arrange
            var dut = await new PwmControllerFixture().CreateDutAsync();

            // Act
            await dut.ChangeDutyCyclePercentAsync(expected);

            // Assert
            dut.DutyCyclePercent.Should().Be(expected);
        }

        [Theory]
        [InlineData(20, 51)]
        [InlineData(100, 255)]
        public async Task SetDirectionAsync_Running_ChangesCurrentPinDutyCycleToZeroThenSpinsUpOtherPin(
            int initialPercentDutyCycle, int expectedDutyCycle)
        {
            // Arrange
            var fixture = new PwmControllerFixture();

            var dut = 
                await fixture
                    .WithDirection(Direction.Forward)
                    .WithDutyCyclePercent(initialPercentDutyCycle)
                    .WithForwardPinDutyCycle(expectedDutyCycle)
                    .CreateDutAsync();

            // Act
            await dut.SetDirectionAsync(Direction.Backward);

            // Assert
            fixture.ForwardPwmPin.CurrentDutyCycle.Should().Be(0);
            fixture.BackwardPwmPin.CurrentDutyCycle.Should().Be(expectedDutyCycle);
        }

        [Theory]
        [InlineData(Direction.Backward, Direction.Forward)]
        [InlineData(Direction.Forward, Direction.Backward)]
        public async Task SetDirectionAsync_default_DirectionPropertyIsUpdated(Direction initialDirection, Direction newDirection)
        {
            // Arrange
            var fixture = new PwmControllerFixture();

            var dut =
                await fixture
                    .WithDirection(initialDirection)
                    .CreateDutAsync();

            // Act
            await dut.SetDirectionAsync(newDirection);

            // Assert
            dut.Direction.Should()
                .Be(newDirection);
        }
    }

    public class PwmControllerFixture
    {
        private int _stepsPerChange;

        private Direction _direction;

        private int _dutyCyclePercent;

        public IPwmPin ForwardPwmPin { get; private set; }

        public IPwmPin BackwardPwmPin { get; private set; }

        // Used for fake pins
        private int _forwardDutyCycle;
        private int _backwardDutyCycle;

        public PwmControllerFixture()
        {
            _direction = Direction.Forward;
            _dutyCyclePercent = 1;
            _stepsPerChange = 3;

        }

        public async Task<PwmController> CreateDutAsync()
        {
            ForwardPwmPin = NewForwardMockPin().Object;
            BackwardPwmPin = NewBackwardMockPin().Object;

            var result = new PwmController(ForwardPwmPin, BackwardPwmPin, _stepsPerChange);

            await result.SetDirectionAsync(_direction);
            await result.ChangeDutyCyclePercentAsync(_dutyCyclePercent);

            return result;
        }

        public PwmControllerFixture WithDirection(Direction direction)
        {
            _direction = direction;

            return this;
        }

        public PwmControllerFixture WithDutyCyclePercent(int dutyCyclePercent)
        {
            _dutyCyclePercent = dutyCyclePercent;

            return this;
        }

        public PwmControllerFixture WithForwardPinDutyCycle(int dutyCycle)
        {
            _forwardDutyCycle = dutyCycle;

            return this;
        }

        private Mock<IPwmPin> NewForwardMockPin()
        {
            var result = new Mock<IPwmPin>();

            // Fake CurrentDutyCycle property
            result
                .SetupGet(m => m.CurrentDutyCycle)
                .Returns(() => _forwardDutyCycle);

            result
                .Setup(m => m.SetActiveDutyCycle(It.IsAny<int>()))
                .Callback(
                    (int newDutyCylcle) =>
                    {
                        _forwardDutyCycle = newDutyCylcle;
                    });

            return result;
        }

        private Mock<IPwmPin> NewBackwardMockPin()
        {
            var result = new Mock<IPwmPin>();

            // Fake CurrentDutyCycle property
            result
                .SetupGet(m => m.CurrentDutyCycle)
                .Returns(() => _backwardDutyCycle);

            result
                .Setup(m => m.SetActiveDutyCycle(It.IsAny<int>()))
                .Callback(
                    (int newDutyCylcle) =>
                    {
                        _backwardDutyCycle = newDutyCylcle;
                    });

            return result;
        }
    }
}