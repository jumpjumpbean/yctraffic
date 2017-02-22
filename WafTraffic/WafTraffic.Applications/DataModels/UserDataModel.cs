using System;
using System.Waf.Applications;
using System.Windows.Input;
using WafTraffic.Domain;

namespace WafTraffic.Applications.DataModels
{
    public class UserDataModel : DataModel
    {
        private readonly Base_User user;


        public UserDataModel(Base_User user)
        {
            if (user == null) { throw new ArgumentNullException("user"); }

            this.user = user;
        }


        public Base_User User { get { return user; } }

    }
}
