using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IPParameterService
    {
        Task<ApiResponse<IEnumerable<PParameterDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<PParameterDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<PParameterDto>> GetByIdAsync(long id);
        Task<ApiResponse<PParameterDto>> CreateAsync(CreatePParameterDto createDto);
        Task<ApiResponse<PParameterDto>> UpdateAsync(long id, UpdatePParameterDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

