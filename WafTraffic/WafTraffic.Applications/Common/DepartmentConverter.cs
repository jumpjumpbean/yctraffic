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
    public class DepartmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string deptName = "";
            try
            {
                int deptId = 0;
                if (value != null)
                {
                    deptId = Int32.Parse(value.ToString());
                    BaseOrganizeManager origanizeService = new BaseOrganizeManager();
                    BaseOrganizeEntity entity = origanizeService.GetEntity(deptId);
                    deptName = entity.FullName;
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
            return deptName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
