using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.OnlineId;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.Web.Syndication;
using ChickNet.Gate;

namespace ChickNet.UI
{
    public class GatestateToColorConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is GateStateModel gateState) || targetType != typeof(Color))
            {
                throw new ArgumentException("Converts only GateStateModel to Color");
            }

            //if (gateState.IsClosed)
            //{
            //    return Color.FromArgb(150, 200, 30, 30);
            //}
            return gateState.IsOpen
                ? Color.FromArgb(150, 30, 200, 30)
                : Color.FromArgb(150, 30, 30, 220);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
