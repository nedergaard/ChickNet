using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ChickNet.Gate;
using System.Threading;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ChickNet
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly ChickNetApp _app;

        //public static readonly DependencyProperty Gate1StateProperty =
        //    DependencyProperty.Register("Gate1State", typeof(IGateState), typeof(MainPage), new PropertyMetadata(string.Empty));

        public GateStateModel Gate1State { get; set; }

        public MainPage()
        {
            InitializeComponent();

            _app = new ChickNetApp();
           
            _app.InitializeHardware().GetAwaiter().GetResult();

            Gate1State = new GateStateModel(_app.Gate1State, SynchronizationContext.Current);
        }


        public void WriteDebug(string s)
        {
            DebugText.Text = s;
        }

        private void ButtonGate1_OnClick(object sender, RoutedEventArgs e)
        {
            // Debug:
            RectangleOutput.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 240, 20, 20));

            if (_app.Gate1State.IsClosed)
            {
                WriteDebug("Opening gate 1");
                _app.GateController.OpenGateAsync(1);
                return;
            }

            WriteDebug("Closing gate 1");
            _app.GateController.CloseGateAsync(1);
        }

        private void ButtonGate2_Click(object sender, RoutedEventArgs e)
        {
            // Debug:
            RectangleOutput.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 20, 230, 20));

            if (_app.Gate2State.IsClosed)
            {
                WriteDebug("Opening gate 2");
                _app.GateController.OpenGateAsync(2);
                return;
            }

            WriteDebug("Closing gate 2");
            _app.GateController.CloseGateAsync(2);
        }
    }
}
