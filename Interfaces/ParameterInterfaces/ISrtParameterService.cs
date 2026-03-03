using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface ISrtParameterService
    {
        Task<ApiResponse<IEnumerable<SrtParameterDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<SrtParameterDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<SrtParameterDto>> GetByIdAsync(long id);
        Task<ApiResponse<SrtParameterDto>> CreateAsync(CreateSrtParameterDto createDto);
        Task<ApiResponse<SrtParameterDto>> UpdateAsync(long id, UpdateSrtParameterDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

