using Dapper;
using SV21T1020546.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020546.DataLayers.SQLServer
{
    public class UserAccountDAL : BaseDAL, IUserAccountDAL
    {
        public UserAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount? Authorize(UserTypes userTypes, string username, string password)
        {

            UserAccount? data = null;
            string sql = "";
            if (userTypes == UserTypes.Employee)
            {
                sql = @"select EmployeeID as UserId,
                                   Email as UserName,
                                   FullName as DisplayName,
                                   Photo,
                                   RoleNames
                            from Employees
                            where Email = @Email and Password = @Password";
            }else if(userTypes == UserTypes.Customer)
            {
                sql = @"select CustomerID as UserId,
                                   Email as UserName,
                                   CustomerName as DisplayName,
                                   N'' as Photo,
                                   N'' as RoleNames
                            from Customers
                            where Email = @Email and Password = @Password";
            }

            using (var connection = OpenConnection())
            {
                var parameters = new
                {
                    Email = username,
                    Password = password
                };

                data = connection.QueryFirstOrDefault<UserAccount>(sql, param: parameters, commandType: System.Data.CommandType.Text);
            }

            return data;
        }

        public bool ChangePassword(UserTypes userTypes, string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
