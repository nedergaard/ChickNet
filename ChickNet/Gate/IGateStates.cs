namespace ChickNet.Gate
{
    public interface IGateStates
    {
        IGateState GetStateOf(int gateNr);
    }
}