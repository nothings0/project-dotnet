﻿using Dapper;
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
                                    Phone,
                                    Address,
		                            N'' as Province,
                                    RoleNames
                            from Employees
                            where Email = @Email and Password = @Password";
            }
            else if(userTypes == UserTypes.Customer)
            {
                sql = @"select CustomerID as UserId,
                                   Email as UserName,
                                   CustomerName as DisplayName,
                                   N'' as Photo,
                                    Phone,
                                    Address,
                                    Province,
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

        public bool ChangePassword(UserTypes userTypes, string username, string oldPassword, string newPassword)
        {
            bool result = false;
            string sql = "";
            if (userTypes == UserTypes.Employee)
            {
                sql = @"update Employees 
                        set Password = @newPassword 
                        where Email = @userName and Password = @oldPassword";
            }
            else if (userTypes == UserTypes.Customer)
            {
                sql = @"update Customers 
                        set Password = @newPassword 
                        where Email = @userName and Password = @oldPassword";
            }
            using (var connection = OpenConnection())
            {
                var parameters = new
                {
                    userName = username,
                    oldPassword = oldPassword,
                    newPassword = newPassword
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
