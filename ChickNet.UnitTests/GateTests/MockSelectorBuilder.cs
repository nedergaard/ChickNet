using ChickNet.Gate;
using Moq;

namespace ChickNet.UnitTests.GateTests
{
    public class MockSelectorBuilder
    {
        private int _selectedNr;

        public MockSelectorBuilder()
        {
            _selectedNr = 1;
        }

        public MockSelectorBuilder WithSelected(int selectedNr)
        {
            _selectedNr = selectedNr;

            return this;
        }

        public Mock<ISelector> Build()
        {
            var result = new Mock<ISelector>();
            result
                .SetupProperty(p => p.Selected, _selectedNr)
                .Setup(p => p.Select(It.IsAny<int>()))
                .Callback(
                    (int nr) =>
                    {
                        result.SetupProperty(p => p.Selected, nr);
                    });
            return result;
        }
    }
}                                               