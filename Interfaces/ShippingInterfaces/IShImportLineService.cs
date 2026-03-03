using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Interfaces
{
    public interface IShImportLineService
    {
        Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<ShImportLineDto>>> GetPagedAsync(PagedRequest request);
        Task<ApiResponse<ShImportLineDto>> GetByIdAsync(long id);
        Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetByHeaderIdAsync(long headerId);
        Task<ApiResponse<IEnumerable<ShImportLineDto>>> GetByLineIdAsync(long lineId);
        Task<ApiResponse<ShImportLineDto>> CreateAsync(CreateShImportLineDto createDto);
        Task<ApiResponse<ShImportLineDto>> UpdateAsync(long id, UpdateShImportLineDto updateDto);
        Task<ApiResponse<bool>> SoftDeleteAsync(long id);
        Task<ApiResponse<ShImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddShImportBarcodeRequestDto request);
        Task<ApiResponse<IEnumerable<ShImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId);
    }
}

