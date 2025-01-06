using SV21T1020546.DataLayers;
using SV21T1020546.DataLayers.SQLServer;
using SV21T1020546.DomainModels;

namespace SV21T1020546.BusinessLayers
{

    public static class UserAccountService
    {
        private static readonly IUserAccountDAL userAccountDB;

        static UserAccountService()
        {
            userAccountDB = new UserAccountDAL(Configuration.ConnectionString);
        }

        public static UserAccount? Authorize(UserTypes userTypes, string username, string password)
        {
            return userAccountDB.Authorize(userTypes, username, password);
        }

        public static bool ChangePassword(UserTypes userTypes, string username, string password, string newPassword)
        {
            return userAccountDB.ChangePassword(userTypes, username, password, newPassword);
        }

        public static int Register(string username, string password)
        {
            return userAccountDB.Register(username, password);
        }
    }
}
