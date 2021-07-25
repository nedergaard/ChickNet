using ChickNetWeb.Gate;
using ChickNetWeb.Hardware;
using ChickNetWeb.Pwm;
using ChickNetWeb.Selection;
using Iot.Device.Board;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChickNetWeb
{
    public class ChickNetApp : IDisposable
    {
        public GateController GateController { get; private set; }

        private GpioController _gpioController;

        private GateStates _gateStates;

        private readonly List<IDisposable> _disposables;

        public IGateState Gate1State => _gateStates.GetStateOf(1);
        public IGateState Gate2State => _gateStates.GetStateOf(2);

        public ChickNetApp()
        {
            _disposables = new List<IDisposable>();
        }

        public async Task InitializeHardware()
        {
            var board = new RaspberryPiBoard();

            _gpioController = new GpioController(PinNumberingScheme.Logical);

            IPin GetInputPin(int pinNr)
            {
                return new GpioPinWrapper(pinNr, PinMode.InputPullUp, _gpioController);
            }

            // Literal pin number values refer to GPIO number

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

            IPwmPin GetPwmChannel(int channelNr)
            {
                var pwmChannel = board.CreatePwmChannel(0, channelNr, dutyCyclePercentage: 0);
                _disposables.Add(pwmChannel);
                return new PwmChannelWrapper(pwmChannel);
            }

            IEnumerable<IPin> GetOutputPins(params int[] pinNumbers)
            {
                foreach (var pinNr in pinNumbers)
                {
                    yield return new GpioPinWrapper(pinNr, PinMode.Output, _gpioController);
                }
            }

            GateController =
                new GateController(
                    // TODO : pins in settings, selector: 6, 13, 19
                    new Selector(GetOutputPins(5, 6, 19)),
                    // TODO : Make sure the prototype and PCB uses the pins that these channels uses.
                    new PwmController(
                        GetPwmChannel(0),
                        GetPwmChannel(1),
                        // TODO : steps per change in settings
                        3),
                    _gateStates);

            // TODO : pins in settings: 24
            _heartBeatPin = GetOutputPins(24).First();
            _heartBeatTimer = new Timer(HandleHeartBeat, _heartBeatState, 100, Timeout.Infinite);
        }


        #region Heart beat

        private Timer _heartBeatTimer;
        private object _heartBeatState;
        private IPin _heartBeatPin;
        private bool _heartBeat;

        private void HandleHeartBeat(object state)
        {
            _heartBeat = !_heartBeat;
            _heartBeatPin.Write(_heartBeat ? PinValue.High : PinValue.Low);

            _heartBeatTimer.Change(500, Timeout.Infinite);
        }

        #endregion

        #region IDisposable

        private bool _isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                // Not thread safe, but should be a problem in this case.
                foreach (var disposable in _disposables.ToList())
                {
                    if (_disposables.Remove(disposable))
                    {
                        disposable.Dispose();
                    }
                }

                _heartBeatTimer.Dispose();
                _heartBeatTimer = null;
                _heartBeatPin.Write(PinValue.Low);
            }

            _isDisposed = true;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
