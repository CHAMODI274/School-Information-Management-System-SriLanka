using SchoolManagementSystem.DTOs.SubjectAllocation;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Services
{
    public class SubjectAllocationService : ISubjectAllocationService
    {
        private readonly ISubjectAllocationRepository _subjectAllocationRepository;

        public SubjectAllocationService(ISubjectAllocationRepository subjectAllocationRepository)
        {
            _subjectAllocationRepository = subjectAllocationRepository;
        }



        // Get all allocations and convert each to a response DTO
        public async Task<IEnumerable<SubjectAllocationResponseDto>> GetAllAsync()
        {
            var allocations = await _subjectAllocationRepository.GetAllAsync();

            return allocations.Select(sa => MapToResponseDto(sa));
        }



        // Get all allocations for a specific teacher
        public async Task<IEnumerable<SubjectAllocationResponseDto>> GetByTeacherAsync(int teacherId)
        {
            var allocations = await _subjectAllocationRepository
                .GetByTeacherAsync(teacherId);

            return allocations.Select(sa => MapToResponseDto(sa));
        }



        // Get all allocations for a specific curriculum entry
        public async Task<IEnumerable<SubjectAllocationResponseDto>> GetByCurriculumAsync(
            int classCurriculumId)
        {
            var allocations = await _subjectAllocationRepository
                .GetByCurriculumAsync(classCurriculumId);

            return allocations.Select(sa => MapToResponseDto(sa));
        }



        // Get one allocation by ID and convert to a response DTO
        public async Task<SubjectAllocationResponseDto?> GetByIdAsync(int id)
        {
            var allocation = await _subjectAllocationRepository.GetByIdAsync(id);

            if (allocation == null) return null; // Return null if not found

            return MapToResponseDto(allocation);
        }



        // Create a brand new subject allocation
        public async Task<SubjectAllocationResponseDto> CreateAsync(
            CreateSubjectAllocationDto dto)
        {
            // Business rule: the same teacher cannot be allocated to the same curriculum entry more than once
            bool allocationExists = await _subjectAllocationRepository
                .AllocationExistsAsync(dto.ClassCurriculumId, dto.TeacherId);

            if (allocationExists)
            {
                throw new InvalidOperationException(
                    "This teacher is already allocated to this subject for this class.");
            }

            // Map the incoming DTO fields onto a new SubjectAllocation model object
            var allocation = new SubjectAllocation
            {
                ClassCurriculumId = dto.ClassCurriculumId,
                TeacherId = dto.TeacherId
            };

            // Ask the repository to add this allocation and save to the database
            await _subjectAllocationRepository.AddAsync(allocation);
            await _subjectAllocationRepository.SaveChangesAsync();

            // Reload the allocation with all nested navigation properties included
            var savedAllocation = await _subjectAllocationRepository
                .GetByIdAsync(allocation.AllocationId);

            return MapToResponseDto(savedAllocation!);
        }



        // Update an existing allocation
        public async Task<SubjectAllocationResponseDto?> UpdateAsync(
            int id, UpdateSubjectAllocationDto dto)
        {
            // First fetch the existing allocation from the database
            var allocation = await _subjectAllocationRepository.GetByIdAsync(id);

            if (allocation == null) return null; // Return null if the allocation doesn't exist

            allocation.TeacherId = dto.TeacherId; // Only the TeacherId can be changed

            // Tell the repository the allocation has changed, then save
            _subjectAllocationRepository.Update(allocation);
            await _subjectAllocationRepository.SaveChangesAsync();

            return MapToResponseDto(allocation);
        }



        // Delete an allocation
        public async Task<bool> DeleteAsync(int id)
        {
            var allocation = await _subjectAllocationRepository.GetByIdAsync(id);

            if (allocation == null) return false;

            _subjectAllocationRepository.Delete(allocation);
            await _subjectAllocationRepository.SaveChangesAsync();

            return true;
        }




        // Converts a raw SubjectAllocation model into a SubjectAllocationResponseDto for the frontend.
        // Note: All nested navigation properties must be loaded (via Include + ThenInclude)
        private static SubjectAllocationResponseDto MapToResponseDto(SubjectAllocation sa)
        {
            return new SubjectAllocationResponseDto
            {
                AllocationId = sa.AllocationId,
                ClassCurriculumId = sa.ClassCurriculumId,

                // come from ClassCurriculum -> Subject (nested navigation)
                SubjectName = sa.ClassCurriculum?.Subject?.SubjectName ?? string.Empty,
                SubjectCode = sa.ClassCurriculum?.Subject?.SubjectCode ?? string.Empty,

                // come from ClassCurriculum -> Class (nested navigation)
                Grade = sa.ClassCurriculum?.Class?.Grade ?? string.Empty,
                Section = sa.ClassCurriculum?.Class?.Section ?? string.Empty,

                // come from ClassCurriculum -> AcademicYear (nested navigation)
                Year = sa.ClassCurriculum?.AcademicYear?.Year ?? string.Empty,

                // come from Teacher (direct navigation)
                TeacherId = sa.TeacherId,
                TeacherName = sa.Teacher?.Name ?? string.Empty
            };
        }
    }
}