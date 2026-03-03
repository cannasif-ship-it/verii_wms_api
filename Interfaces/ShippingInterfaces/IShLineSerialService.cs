using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IShLineSerialService
    {
        Task<ApiResponse<IEnumerable<ShLineSerialDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<ShLineSerialDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<ShLineSerialDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<ShLineSerialDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<ShLineSerialDto>> CreateAsync(CreateShLineSerialDto createDto);
        Task<ApiResponse<ShLineSerialDto>> UpdateAsync(long id, UpdateShLineSerialDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
    }
}

