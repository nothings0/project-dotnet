using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020546.BusinessLayers
{
    public class Configuration
    {
        private static string connectionString = "";

        public static void Initialize(string connectionString)
        {
            Configuration.connectionString = connectionString;
        }

        public static string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }
    }
}
