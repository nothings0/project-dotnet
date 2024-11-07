using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020546.DataLayers
{
    public interface ICommonDAL<T> where T : class
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách dữ liệu dạng phân trang
        /// </summary>
        /// <param name="page"></param> Trang can hien thi
        /// <param name="pageSize"></param>So dong trong 1 trang
        /// <param name="searchValue"></param>Gia tri can tim
        /// <returns></returns>
        List<T> List(int page = 1, int pageSize = 0, string searchValue = "");

        /// <summary>
        /// dem so dong du lieu tim duoc
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue = "");

        /// <summary>
        /// bo sung du lieu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);

        /// <summary>
        /// cap nhat du lieu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);

        /// <summary>
        /// xoa du lieu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);

        /// <summary>
        /// get data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T? Get(int id);

        /// <summary>
        /// kiem tra du lieu co khoa id co lien quan den bang khac khong
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool InUsed(int id);
    }
}
