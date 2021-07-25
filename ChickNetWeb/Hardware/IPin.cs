using System.Device.Gpio;

namespace ChickNetWeb.Hardware
{
    public interface IPin
    {
        PinValue Read();

        void Write(PinValue newValue);
    }
}
