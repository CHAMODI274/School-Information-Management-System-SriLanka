using SchoolManagementSystem.DTOs.Promotion;

namespace SchoolManagementSystem.Interfaces
{
    public interface IPromotionService
    {
        Task<PromotionResultDto> PromoteStudentAsync(PromoteStudentDto dto);

        Task<BulkPromotionResultDto> BulkPromoteAsync(BulkPromoteDto dto);
    }
}