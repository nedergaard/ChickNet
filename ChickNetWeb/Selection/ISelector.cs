namespace ChickNetWeb.Selection
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
