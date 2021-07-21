namespace ChickNetWeb.Gate
{
    public interface IGateStates
    {
        IGateState GetStateOf(int gateNr);
    }
}