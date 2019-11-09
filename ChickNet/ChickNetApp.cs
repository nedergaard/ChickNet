using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Gpio;
using Windows.Devices.Pwm;
using ChickNet.Gate;
using ChickNet.Pwm;
using ChickNet.Selection;
using Microsoft.IoT.Lightning.Providers;
using PwmController = ChickNet.Pwm.PwmController;

namespace ChickNet
{
    public class ChickNetApp
    {
        public GateController GateController { get; private set; }

        private readonly GpioController _gpioController;

        private GateStates _gateStates;

        public IGateState Gate1State => _gateStates.GetStateOf(1);
        public IGateState Gate2State => _gateStates.GetStateOf(2);

        public ChickNetApp()
        {
            _gpioController = GpioController.GetDefault();
        }

        public async Task InitializeHardware()
        {
            if (LightningProvider.IsLightningEnabled)
            {
                LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();

                var pwmControllers = await Windows.Devices.Pwm.PwmController.GetControllersAsync(LightningPwmProvider.GetPwmProvider());
                var pwmController = pwmControllers[1]; // the on-device controller
                pwmController.SetDesiredFrequency(100);

                _gateStates = new GateStates();
                _gateStates.Add(
                    // Center switches (on bread board)
                    new GateState(
                        // Yellow button, Closed
                        new GpioPinWrapper(_gpioController.OpenPin(27)),
                        // Red button, Open
                        new GpioPinWrapper(_gpioController.OpenPin(22))),
                    1);
                _gateStates.Add(
                    // Top switches (on bread board)
                    new GateState(
                        // Yellow button, Closed
                        new GpioPinWrapper(_gpioController.OpenPin(4)),
                        // Red button, Open
                        new GpioPinWrapper(_gpioController.OpenPin(17))),
                    2);

                IPwmPin GetPwmPin(int pinNr)
                {
                    var pin = _gpioController.OpenPin(pinNr);
                    pin.SetDriveMode(GpioPinDriveMode.Output);
                    return new PwmPinWrapper(pwmController.OpenPin(pinNr));
                }

                GateController =
                    new GateController(
                        // TODO : pins in settings, selector: 6, 13, 19
                        new Selector(GetPins(6, 13, 19)),
                        new PwmController(
                            // TODO : pins in settings: 20, 21
                            GetPwmPin(20),
                            GetPwmPin(21),
                            20),
                        _gateStates);
            }
        }

        private IEnumerable<IPin> GetPins(params int[] pinNumbers)
        {
            for (var pinNr = 0; pinNr < pinNumbers.Length; pinNr++)
            {
                var pin = _gpioController.OpenPin(pinNr);
                pin.SetDriveMode(GpioPinDriveMode.Output);

                yield return new GpioPinWrapper(pin);
            }
        }
    }
}
