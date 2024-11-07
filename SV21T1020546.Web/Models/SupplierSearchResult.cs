﻿using SV21T1020546.DomainModels;

namespace SV21T1020546.Web.Models
{
    public class SupplierSearchResult : PaginationSearchResult
    {
        public required List<Supplier> Data { get; set; }
    }
}