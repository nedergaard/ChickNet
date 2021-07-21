using System.Threading.Tasks;

namespace ChickNetWeb.Pwm
{
    /// <summary>
    /// Controller for Pulse Width Modulation outputs
    /// </summary>
    public interface IPwmController
    {
        Direction Direction { get; }

        Task SetDirectionAsync(Direction newDirection);

        int DutyCyclePercent { get; }

        /// <summary>
        /// Smoothly changes the <see cref="DutyCyclePercent"/> to a new value.
        /// </summary>
        /// <param name="percent">New DutyCyclePercent percentage</param>
        Task ChangeDutyCyclePercentAsync(int percent);

        /// <summary>
        /// Steps duty cycle to 0 rapidly
        /// </summary>
        Task StopAsync();

        // EmergencyStop()  Sets Duty cycle to 0 immediately
    }

    public enum Direction
    {
        Forward,
        Backward,
    }
}
