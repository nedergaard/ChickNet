using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Media;
using ChickNet.Gate;
using ChickNet.Selection;
using ChickNet.UnitTests.GateTests;
using FluentAssertions;
using Moq;
using Xunit;

namespace ChickNet.UnitTests.StateTests
{
    public class GateStateTests
    {
        // [UnitOfWork_StateUnderTest_ExpectedBehavior]
        // Low indicates the switch is closed, i.e. the gate is next to the switch.
        [Theory]
        // Gate is closed
        [InlineData(GpioPinValue.Low, GpioPinValue.Low, false)]
        // Gate is half open
        [InlineData(GpioPinValue.High, GpioPinValue.Low, false)]
        // Gate is fully open, none of the switches are activated
        [InlineData(GpioPinValue.High, GpioPinValue.High, true)]
        // "Impossible" state
        [InlineData(GpioPinValue.Low, GpioPinValue.High, false)]
        public void IsOpen_GateClosed_ReturnsExpected(GpioPinValue closedPinValue, GpioPinValue openPinValue, bool expected)
        {
            // Arrange
            var closedSwithPin = new Mock<IPin>();
            closedSwithPin
                .Setup(p => p.Read())
                .Returns(closedPinValue);

            var openSwithPin = new Mock<IPin>();
            openSwithPin
                .Setup(p => p.Read())
                .Returns(openPinValue);

            var dut = new GateState(closedSwithPin.Object, openSwithPin.Object);

            // Act
            var actual = dut.IsOpen;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        // Gate is closed
        [InlineData(GpioPinValue.Low, GpioPinValue.Low, true)]
        // Gate is half open
        [InlineData(GpioPinValue.High, GpioPinValue.Low, false)]
        // Gate is fully open, none of the switches are activated
        [InlineData(GpioPinValue.High, GpioPinValue.High, false)]
        // "Impossible" state
        [InlineData(GpioPinValue.Low, GpioPinValue.High, false)]
        public void IsClosed_GateClosed_ReturnsExpected(GpioPinValue closedPinValue, GpioPinValue openPinValue, bool expected)
        {
            // Arrange
            var closedSwithPin = new Mock<IPin>();
            closedSwithPin
                .Setup(p => p.Read())
                .Returns(closedPinValue);

            var openSwithPin = new Mock<IPin>();
            openSwithPin
                .Setup(p => p.Read())
                .Returns(openPinValue);

            var dut = new GateState(closedSwithPin.Object, openSwithPin.Object);

            // Act
            var actual = dut.IsClosed;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
