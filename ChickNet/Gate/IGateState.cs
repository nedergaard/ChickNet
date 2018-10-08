namespace ChickNet.Gate
{
    public interface IGateState
    {
        // Indicates that the gate is fully open.
        bool IsOpen { get; }

        // Indicates that the gate is completely closed.
        bool IsClosed { get; }
    }
}
