﻿using System;
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

        private GpioController _gpioController;

        private GateStates _gateStates;

        public IGateState Gate1State => _gateStates.GetStateOf(1);
        public IGateState Gate2State => _gateStates.GetStateOf(2);

        public ChickNetApp()
        {
        }

        public async Task InitializeHardware()
        {
            if (LightningProvider.IsLightningEnabled)
            {
                LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();

                var pmwProvider = LightningPwmProvider.GetPwmProvider();

//                var pwmControllers = await Windows.Devices.Pwm.PwmController.GetControllersAsync(pmwProvider);
                var pwmControllers = Windows.Devices.Pwm.PwmController.GetControllersAsync(pmwProvider).GetAwaiter().GetResult();
                var pwmController = pwmControllers[1]; // the on-device controller

                // This seems to go bad
                //pwmController.SetDesiredFrequency(100);

                _gpioController = await GpioController.GetDefaultAsync();

                IPin GetInputPin(int pinNr)
                {
                    return new GpioPinWrapper(_gpioController.OpenPin(pinNr, GpioSharingMode.Exclusive), GpioPinDriveMode.InputPullUp);
                }

                _gateStates = new GateStates();
                _gateStates.Add(
                    // Center switches (on bread board)
                    new GateState(
                        // Yellow button, Closed
                        GetInputPin(27),
                        // Red button, Open
                        GetInputPin(22)),
                    1);
                _gateStates.Add(
                    // Top switches (on bread board)
                    new GateState(
                        // Yellow button, Closed
                        GetInputPin(4),
                        // Red button, Open
                        GetInputPin(17)),
                    2);

                IPwmPin GetPwmPin(int pinNr)
                {
                    //var pin = _gpioController.OpenPin(pinNr);
                    //pin.SetDriveMode(GpioPinDriveMode.Output);
                    return new PwmPinWrapper(pwmController.OpenPin(pinNr));
                }

                GateController =
                    new GateController(
                        // TODO : pins in settings, selector: 6, 13, 19
                        new Selector(GetOutputPins(6, 13, 19)),
                        new PwmController(
                            // TODO : pins in settings: 20, 21
                            GetPwmPin(20),
                            GetPwmPin(21),
                            8),
                        _gateStates);
            }
        }

        private IEnumerable<IPin> GetOutputPins(params int[] pinNumbers)
        {
            for (var pinNrIndex = 0; pinNrIndex < pinNumbers.Length; pinNrIndex++)
            {
                var pin = _gpioController.OpenPin(pinNumbers[pinNrIndex], GpioSharingMode.Exclusive);

                yield return new GpioPinWrapper(pin, GpioPinDriveMode.Output);
            }
        }
    }
}
