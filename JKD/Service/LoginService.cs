using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKD.DB;
using JKD.Models;
namespace JKD.Service
{
    public class LoginService
    {
        public UserManager um = new UserManager();
        public LoginStatus login(string username,string psw)
        {
            if (!SqlHelper.IsSuccessConn())
            {
                return LoginStatus.ConnectionErr;
            }
            User u = new UserManager().GetUser(username);
            if(u == null)
            {
                
                return LoginStatus.UsernameErr;
            }
            if(u.psw != psw)
            {
                return LoginStatus.PasswordErr;
            }
            AppDomain.CurrentDomain.SetData("user", u);
            return LoginStatus.Success;
        }

        public RegisterStatus Register(string name,string username)
        {
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(username))
            {
                return RegisterStatus.Nullerr;
            }
            List<User> users = um.GetList();
            if (users.Select(it => it.name).ToList<string>().Contains(name))
            {
                return RegisterStatus.NameDuplicate;
            }else if (users.Select(it=>it.username).ToList<string>().Contains(username))
            {
                return RegisterStatus.UsernameDuplicate;
            }
            return RegisterStatus.Success;

        }

    }
    public enum LoginStatus
    {
        Success,
        UsernameErr,
        PasswordErr,
        ConnectionErr
    }
    public enum RegisterStatus
    {
        NameDuplicate,
        UsernameDuplicate,
        Nullerr,
        Success
    }
}
