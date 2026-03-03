using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPtParameterService
    {
        Task<ApiResponse<IEnumerable<PtParameterDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<PtParameterDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<PtParameterDto>> GetByIdAsync(long id);
        Task<ApiResponse<PtParameterDto>> CreateAsync(CreatePtParameterDto createDto);
        Task<ApiResponse<PtParameterDto>> UpdateAsync(long id, UpdatePtParameterDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

