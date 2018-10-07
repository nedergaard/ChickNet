namespace ChickNet.Gate
{
    /// <summary>
    /// Controller for Pulse Width Modulation outputs
    /// </summary>
    public interface IPwmController
    {
        Direction Direction { get; set; }

        int DutyCycle { get; }

        /// <summary>
        /// Smoothly changes the <see cref="DutyCycle"/> to a new value.
        /// </summary>
        /// <param name="percent">New DutyCycle percentage</param>
        void ChangeDutyCycleTo(int percent);

        // Stop()  Steps duty cycle to 0 rapidly
        // EmergencyStop()  Sets Duty cycle to 0 immediately
    }

    public enum Direction
    {
        Forward,
        Backward,
    }
}
