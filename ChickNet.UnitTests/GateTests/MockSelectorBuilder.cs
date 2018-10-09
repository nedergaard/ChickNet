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

        public MockSelector Build()
        {
            return 
                new MockSelector()
                {
                    SelectedNr = _selectedNr
                };
        }
    }
}                                               