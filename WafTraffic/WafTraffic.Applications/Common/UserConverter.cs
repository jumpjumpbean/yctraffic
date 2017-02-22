using System;
using System.Windows;
using System.Windows.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.ComponentModel.Composition;
using DotNet.Business;

namespace WafTraffic.Applications.Common
{
    [Export]
    public class UserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string userName = "";
            try
            {
                int userId = 0;
                if (value != null)
                {
                    userId = Int32.Parse(value.ToString());
                    BaseUserManager userService = new BaseUserManager();
                    BaseUserEntity entity = userService.GetEntity(userId);
                    userName = entity.RealName;
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
            return userName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
