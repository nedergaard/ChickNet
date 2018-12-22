using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using ChickNet.Gate;
using ChickNet.Selection;
using ChickNet.UnitTests.GateTests;
using FluentAssertions;
using Moq;
using Xunit;

namespace ChickNet.UnitTests.StateTests
{
    public class GateStatesTests
    {
        // Generally the specifc gate numbers used in these tests are not significant.

        // [UnitOfWork_StateUnderTest_ExpectedBehavior]
        [Fact]
        public void Add_Empty_CanBeRetrievedByGatenr()
        {
            // Arrange
            var mockGate = new Mock<IGateState>();
            var someGatenr = 4711;

            var dut = new GateStates();

            var expected = mockGate.Object;

            // Act
            dut.Add(expected, someGatenr);
            var actual = dut.GetStateOf(someGatenr);

            // Assert
            Assert.Same(expected, actual);
        }

        [Fact]
        public void Add_ContainsOtherGates_CanBeRetrievedByGatenr()
        {
            // Arrange
            var mockGate = new Mock<IGateState>();
            var someGatenr = 2187;

            var dut = new GateStates();
            // Add other gates - gate numbers are not significant, except that they be unique.
            dut.Add(NewGate(), 1138);
            dut.Add(NewGate(), 4711);

            var expected = mockGate.Object;

            // Act
            dut.Add(expected, someGatenr);
            var actual = dut.GetStateOf(someGatenr);

            // Assert
            Assert.Same(expected, actual);
        }

        [Fact]
        public void Add_AlreadyContainsGateWithSameGatenr_ThrowsArgumentException()
        {
            // Arrange
            var someNonuniqueGatenr = 3263827;

            var dut = new GateStates();
            dut.Add(NewGate(), someNonuniqueGatenr);

            // Act
            var actual = Assert.Throws<ArgumentException>(() => dut.Add(NewGate(), someNonuniqueGatenr));

            // Assert
            actual.Message.Should()
                .Contain($"{someNonuniqueGatenr}");
        }

        [Fact]
        public void GetStateOf_DoesNotContainRequestedGate_ThrowsKeyNotFoundException()
        {
            // Arrange
            var someNonexistingGatenr = 75159;

            var dut = new GateStates();

            // Act
            var actual = Assert.Throws<KeyNotFoundException>(() => dut.GetStateOf(someNonexistingGatenr));

            // Assert
            actual.Message.Should()
                .Contain($"{someNonexistingGatenr}");
        }

        public IGateState NewGate()
        {
            return new Mock<IGateState>().Object;
        }
    }
}
