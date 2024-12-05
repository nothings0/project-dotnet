using SV21T1020546.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020546.DataLayers
{
    public interface IUserAccountDAL
    {
        UserAccount? Authorize(UserTypes userTypes, string username, string password);

        bool ChangePassword(UserTypes userTypes, string username, string password);
    }

    public enum UserTypes
    {
        Employee,
        Customer
    }
}
