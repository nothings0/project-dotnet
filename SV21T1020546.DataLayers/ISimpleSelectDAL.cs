using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020546.DataLayers
{
    public interface ISimpleSelectDAL<T> where T : class
    {
        /// <summary>
        /// select toan bo du lieu 1 bang
        /// </summary>
        /// <returns></returns>
        List<T> List();
    }
}
