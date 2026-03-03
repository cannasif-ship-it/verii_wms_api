using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISitParameterService
    {
        Task<ApiResponse<IEnumerable<SitParameterDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<SitParameterDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<SitParameterDto>> GetByIdAsync(long id);
        Task<ApiResponse<SitParameterDto>> CreateAsync(CreateSitParameterDto createDto);
        Task<ApiResponse<SitParameterDto>> UpdateAsync(long id, UpdateSitParameterDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

