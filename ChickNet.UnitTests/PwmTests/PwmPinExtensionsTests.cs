using System.Collections.Generic;
using System.Threading.Tasks;
using ChickNet.Pwm;
using FluentAssertions;
using Moq;
using Xunit;

namespace ChickNet.UnitTests.PwmTests
{
    public class PwmPinExtensionsTests
    {
        // [UnitOfWork_StateUnderTest_ExpectedBehavior]
        [Fact]
        public async Task ChangeDutyCycleInSteps_AtZero_AcceleratesInSteps()
        {
            // Arrange
            var fixture = new PwmPinFixture();
            var mockPin = fixture.CreateFakePin();

            var expected = new List<int> { 10, 20, 30, 40, 50 };

            // Act
            await mockPin.ChangeDutyCycleInStepsAsync(targetDutyCycle: 50, stepsPerChange: 5);

            // Assert
            fixture.DutycycleChanges.Should()
                .BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Fact]
        public async Task ChangeDutyCycleInSteps_AtSpeed_DeAcceleratesInSteps()
        {
            // Arrange
            var fixture = new PwmPinFixture();
            var mockPin = 
                fixture
                    .WithCurrentDutyCycle(90)
                    .CreateFakePin();

            var expected = new List<int> { 80, 70, 60, 50, 40 };

            // Act
            await mockPin.ChangeDutyCycleInStepsAsync(targetDutyCycle: 40, stepsPerChange: 5);

            // Assert
            fixture.DutycycleChanges.Should()
                .BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Fact]
        public async Task ChangeDutyCycleInSteps_ChangeSmallerThan1PerStep_AcceleratesInFewerSteps()
        {
            // Arrange
            var fixture = new PwmPinFixture();
            var mockPin = fixture.CreateFakePin();

            var expected = new List<int> { 1, 2, 3, 4 };

            // Act
            await mockPin.ChangeDutyCycleInStepsAsync(targetDutyCycle: 4, stepsPerChange: 5);

            // Assert
            fixture.DutycycleChanges.Should()
                .BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(50)]
        [InlineData(99)]
        [InlineData(100)]
        public async Task ChangeDutyCycleInSteps_AtTargetDutycycleAlready_DoesNothing(int dutyCycle)
        {
            // Arrange
            var fixture = new PwmPinFixture();
            var mockPin = 
                fixture
                    .WithCurrentDutyCycle(dutyCycle)
                    .CreateFakePin();

            // Act
            await mockPin.ChangeDutyCycleInStepsAsync(dutyCycle);

            // Assert
            fixture.DutycycleChanges.Should()
                .BeEmpty("'changing' to current value could cause an infinite loop.");
        }

        [Fact]
        public async Task Stop_AlreadyStopped_DoesNothing()
        {
            // Arrange
            var fixture = new PwmPinFixture();
            var mockPin =
                fixture
                    .WithCurrentDutyCycle(0)
                    .CreateFakePin();

            // Act
            await mockPin.Stop();

            // Assert
            fixture.DutycycleChanges.Should()
                .BeEmpty("'changing' to current value could cause an infinite loop.");
        }

        [Theory]
        [InlineData(150, 100, 50, 0)]
        [InlineData(12, 8, 4, 0)]
        public async Task Stop_AtSpeed_DeAcceleratesInAFewSteps(int startSpeed, int expectedStep1, int expectedStep2, int expectedStep3)
        {
            // Arrange
            var fixture = new PwmPinFixture();
            var mockPin =
                fixture
                    .WithCurrentDutyCycle(startSpeed)
                    .CreateFakePin();

            var expected = new List<int> { expectedStep1, expectedStep2, expectedStep3 };

            // Act
            await mockPin.Stop();

            // Assert
            fixture.DutycycleChanges.Should()
                .BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }
    }

    public class PwmPinFixture
    {
        private int _currentDutyCycle;

        public PwmPinFixture()
        {
            DutycycleChanges = new List<int>();

            _currentDutyCycle = 0;
        }

        public List<int> DutycycleChanges { get; }

        public PwmPinFixture WithCurrentDutyCycle(int dutyCycle)
        {
            _currentDutyCycle = dutyCycle;

            return this;
        }

        public IPwmPin CreateFakePin()
        {
            var result = new Mock<IPwmPin>();

            // Fake CurrentDutyCyclePercent property
            result
                .SetupGet(m => m.CurrentDutyCyclePercent)
                .Returns(() => _currentDutyCycle);

            // Record changes made to duty cycle.
            result
                .Setup(m => m.SetActiveDutyCyclePercent(It.IsAny<int>()))
                .Callback(
                    (int newDutyCylcle) =>
                    {
                        DutycycleChanges.Add(newDutyCylcle);
                        //stopWatch.Restart();
                        _currentDutyCycle = newDutyCylcle;
                    });

            return result.Object;
        }
    }
}

