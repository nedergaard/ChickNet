using System.Collections.Generic;
using System.Linq;
using ChickNet.Selection;

namespace ChickNet.UnitTests.SelectionTests
{
    public class SelectorTests
    {
        public void Select1_7selected_SetAllPinsLow()
        {
            // Arrange
            

            // Act


            // Assert
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
}
