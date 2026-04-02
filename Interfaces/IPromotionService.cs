using SchoolManagementSystem.DTOs.Promotion;

namespace SchoolManagementSystem.Interfaces
{
    public interface IPromotionService
    {
        Task<PromotionResultDto> PromoteStudentAsync(PromoteStudentDto dto);

        Task<ClassPromotionResultDto> ClassPromotionAsync(ClassPromoteDto dto);

        Task<SchoolPromotionResultDto> PromoteWholeSchoolAsync(SchoolPromotionDto dto);
    }
}