﻿namespace SV21T1020546.Shop.Models
{
    /// <summary>
    /// Lưu giữ các thông tin đầu vào để sửu dụng cho các chức năng tìm kiếm và hiển thị dữ liệu dưới dạng phân trang
    /// </summary>
    public class PaginationSearchInput
    {
        /// <summary>
        /// TRang cần hiển thị
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// Số dòng hiển thị
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Chuỗi giá trị cần tìm kiếm
        /// </summary>
        public string SearchValue { get; set; } = "";
    }
}