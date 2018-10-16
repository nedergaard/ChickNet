using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Gpio;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using ChickNet.Selection;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace ChickNet.UnitTests.SelectionTests
{
    public class SelectorTests
    {
        [Theory]
        [InlineData(0, false, false, false)]
        [InlineData(1, true, false, false)]
        [InlineData(2, false, true, false)]
        [InlineData(4, false, false, true)]
        [InlineData(6, false, true, true)]
        [InlineData(7, true, true, true)]
        public void Select_0selected_SetPinOutput(int selected, bool expectedPin1, bool expectedPin2, bool expectedPin3)
        {
            // Arrange
            var fixture =
                new SelectorFixture()
                    .WithOutputPins(3);

            var expected =
                new ListFakePinBuilder()
                    .WithPin(expectedPin1)
                    .WithPin(expectedPin2)
                    .WithPin(expectedPin3)
                    .Build();

            // Act
            fixture.CreateDut().Select(selected);

            // Assert
            fixture.OutputPins.Should()
                .BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(7)]
        public void Select_0selected_SetSelected(int expected)
        {
            // Arrange
            var fixture =
                new SelectorFixture()
                    .WithOutputPins(3);


            var dut = fixture.CreateDut();

            // Act
            dut.Select(expected);

            // Assert
            Assert.Equal(expected, dut.Selected);
        }
    }

    public class SelectorFixture
    {
        public List<FakePin> OutputPins { get; }

        public SelectorFixture()
        {
            OutputPins = new List<FakePin>();
        }

        public SelectorFixture WithOutputPins(int numberOfPins)
        {
            OutputPins.AddRange(
                Enumerable.Range(0, numberOfPins)
                    .Select(_ => new FakePin()));

            return this;
        }

        public Selector CreateDut()
        {
            return new Selector(OutputPins);
        }
    }

    public class ListFakePinBuilder
    {
        private List<FakePin> _result;

        public ListFakePinBuilder()
        {
            _result = new List<FakePin>();
        }

        public ListFakePinBuilder WithLowPin()
        {
            return WithPin(GpioPinValue.Low);
        }

        public ListFakePinBuilder WithHighPin()
        {
            return WithPin(GpioPinValue.High);
        }

        public ListFakePinBuilder WithPin(bool isHigh)
        {
            return WithPin(isHigh ? GpioPinValue.High : GpioPinValue.Low);
        }

        public ListFakePinBuilder WithPin(GpioPinValue currentValue)
        {
            _result.Add(new FakePin { CurrrentPinValue = currentValue });

            return this;
        }

        public List<FakePin> Build()
        {
            return _result;
        }
    }
}
