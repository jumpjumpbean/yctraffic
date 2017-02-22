using System;
using System.Windows;
using System.Windows.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.ComponentModel.Composition;

namespace WafTraffic.Applications.Common
{
    [Export]
    public class IntToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int flag = 0;
            if (value != null)
            {
                flag = Int32.Parse(value.ToString());
            }
            if (flag > 0)
            {
                return "是";
            }
            else
            {
                return "否";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
