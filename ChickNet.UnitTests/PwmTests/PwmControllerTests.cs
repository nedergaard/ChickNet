using System.Collections.Generic;
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

            var dut = fixture.CreateDut();

            // Act
            await dut.ChangeDutyCyclePercentAsync(percentInput);

            // Assert
            fixture.ForwardPwmPin.CurrentDutyCycle.Should().Be(expected);
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

            ForwardPwmPin = NewForwardMockPin().Object;
            BackwardPwmPin = NewBackwardMockPin().Object;
        }

        public PwmController CreateDut()
        {
            (_direction == Direction.Forward ? ForwardPwmPin : BackwardPwmPin).SetActiveDutyCycle(_dutyCyclePercent);

            var result = 
                new PwmController(ForwardPwmPin, BackwardPwmPin, _stepsPerChange)
                {
                    Direction = _direction
                };

            return result;
        }

        private Mock<IPwmPin> NewForwardMockPin()
        {
            var result = new Mock<IPwmPin>();

            // Fake CurrentDutyCycle property
            result
                .SetupGet(m => m.CurrentDutyCycle)
                .Returns(() => _forwardDutyCycle);

            // Record changes made to duty cycle.
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

            // Record changes made to duty cycle.
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