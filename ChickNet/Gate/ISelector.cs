namespace ChickNet.Gate
{
    /// <summary>
    /// Selector
    /// </summary>
    public interface ISelector
    {
        int Selected { get; }

        void Select(int itemNr);
    }
}
