using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGrParameterService
    {
        Task<ApiResponse<IEnumerable<GrParameterDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<GrParameterDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<GrParameterDto>> GetByIdAsync(long id);
        Task<ApiResponse<GrParameterDto>> CreateAsync(CreateGrParameterDto createDto);
        Task<ApiResponse<GrParameterDto>> UpdateAsync(long id, UpdateGrParameterDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

