namespace ChickNet.Gate
{
    public interface IGateStates
    {
        IGateState GetStateOf(int gateNr);
    }

    public class GateStates : IGateStates
    {
        #region Implementation of IGateStates

        /// <inheritdoc />
        public IGateState GetStateOf(int gateNr)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}