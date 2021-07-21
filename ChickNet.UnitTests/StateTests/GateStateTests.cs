using System.Collections.Generic;
using System.Device.Gpio;
using ChickNetWeb.Gate;
using ChickNetWeb.Selection;
using Moq;
using Xunit;

namespace ChickNet.UnitTests.StateTests
{
    public class GateStateTests
    {
        public static IEnumerable<object[]> Get_IsOpen_TestData =>
            new List<object[]>
            {
                // Low indicates the switch is closed, i.e. the gate is next to the switch.

                // Gate is closed
                new object[] { PinValue.Low, PinValue.Low, false },
                // Gate is half open
                new object[] { PinValue.High, PinValue.Low, false },
                // Gate is fully open, none of the switches are activated
                new object[] { PinValue.High, PinValue.High, true },
                // "Impossible" state
                new object[] { PinValue.Low, PinValue.High, false },
            };

        // [UnitOfWork_StateUnderTest_ExpectedBehavior]
        [Theory]
        // Gate is closed
        [MemberData(nameof(Get_IsOpen_TestData))]
        public void IsOpen_GateClosed_ReturnsExpected(PinValue closedPinValue, PinValue openPinValue, bool expected)
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

        public static IEnumerable<object[]> Get_IsClosed_TestData =>
            new List<object[]>
            {
                // Low indicates the switch is closed, i.e. the gate is next to the switch.

                // Gate is closed
                new object[] { PinValue.Low, PinValue.Low, true },
                // Gate is half open
                new object[] { PinValue.High, PinValue.Low, false },
                // Gate is fully open, none of the switches are activated
                new object[] { PinValue.High, PinValue.High, false },
                // "Impossible" state
                new object[] { PinValue.Low, PinValue.High, false },
            };

        [Theory]
        [MemberData(nameof(Get_IsClosed_TestData))]
        public void IsClosed_GateClosed_ReturnsExpected(PinValue closedPinValue, PinValue openPinValue, bool expected)
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
