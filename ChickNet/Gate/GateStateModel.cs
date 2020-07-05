using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChickNet.Gate
{
    public class GateStateModel : INotifyPropertyChanged, IDisposable
    {
        private readonly IGateState _gateState;
        private IDisposable _subscription;

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isOpen;

        public bool IsOpen 
        { 
            get => _isOpen;
            set
            {
                if (_isOpen == value)
                {
                    return;
                }
                _isOpen = value;
                OnPropertyChanged("IsOpen");
            }
        }

        public SynchronizationContext _synchronizationContext { get; }

        private void OnPropertyChanged(string propertyName)
        {
            _synchronizationContext.Post(c => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)), null);
        }

        public GateStateModel(IGateState gateState, SynchronizationContext synchronizationContext)
        {
            _gateState = gateState;
            _synchronizationContext = synchronizationContext;

            _subscription =
                Observable
                    .Interval(TimeSpan.FromSeconds(1))
                    .Subscribe(s => OnTimer());
        }

        private object OnTimer()
        {
            IsOpen = _gateState.IsOpen;

            return null;
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                _subscription?.Dispose();
                _subscription = null;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
