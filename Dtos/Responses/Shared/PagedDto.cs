using ApiCoreDapperCrudPagination.Models;

namespace ApiCoreDapperCrudPagination.Dtos.Responses.Shared
{
    public abstract class PagedDto : SuccessResponse
    {
        public PageMeta PageMeta { get; set; }
    }
}