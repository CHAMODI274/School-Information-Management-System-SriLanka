using SchoolManagementSystem.DTOs.Promotion;

namespace SchoolManagementSystem.Interfaces
{
    public interface IPromotionService
    {
        Task<PromotionResultDto> PromoteStudentAsync(PromoteStudentDto dto);

        Task<ClassPromotionResultDto> ClassPromoteAsync(ClassPromoteDto dto);

        Task<SchoolPromotionResultDto> PromoteWholeSchoolAsync(SchoolPromotionDto dto);
    }
}