using SchoolManagementSystem.DTOs.AdministrativeStaff;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Services
{
    // contains all BUSINESS LOGIC
    public class AdministrativeStaffService : IAdministrativeStaffService
    {
        private readonly IAdministrativeStaffRepository _administrativeStaffRepository;

        public AdministrativeStaffService(IAdministrativeStaffRepository administrativeStaffRepository)
        {
            _administrativeStaffRepository = administrativeStaffRepository;
        }



        // Get all administrative staff members and convert each to a response DTO
        public async Task<IEnumerable<AdministrativeStaffResponseDto>> GetAllAsync()
        {
            // Ask the repository for all raw Administrative Staff model objects
            var staffList = await _administrativeStaffRepository.GetAllAsync();

            // Convert each AdministrativeStaff model to a AdminisrativeStaffResponseDto
            return staffList.Select(n => MapToResponseDto(n));
        }



        // Get one administrative staff member by ID and convert to a response DTO
        public async Task<AdministrativeStaffResponseDto?> GetByIdAsync(int id)
        {
            var staff = await _administrativeStaffRepository.GetByIdAsync(id);

            // Return null if not found
            if (staff == null) return null;

            return MapToResponseDto(staff);
        }



        // Create a new administrative staff record
        public async Task<AdministrativeStaffResponseDto> CreateAsync(CreateAdministrativeStaffDto dto)
        {
            // Business rule: each staff member must have a unique NIC number
            bool nicTaken = await _administrativeStaffRepository.NICExistsAsync(dto.NIC);
            if (nicTaken)
            {
                throw new InvalidOperationException(
                    $"A staff member with NIC '{dto.NIC}' already exists.");
            }

            // Business rule: each staff member must have a unique email address
            bool emailTaken = await _administrativeStaffRepository.EmailExistsAsync(dto.Email);
            if (emailTaken)
            {
                throw new InvalidOperationException(
                    $"A staff member with email '{dto.Email}' already exists.");
            }

            // Map the incoming DTO fields onto a new AdministrativeStaff model object
            var staff = new AdministrativeStaff
            {
                Title = dto.Title,
                Name = dto.Name,
                NIC = dto.NIC,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email,
                DateOfJoining = dto.DateOfJoining,
                EmploymentStatus = dto.EmploymentStatus,
                EmployeeType = dto.EmployeeType,
                Position = dto.Position
            };

            // Ask the repository to add this staff member and save to the database
            await _administrativeStaffRepository.AddAsync(staff);
            await _administrativeStaffRepository.SaveChangesAsync();
            // After SaveChangesAsync(), EF Core fills in the EmployeeId automatically

            // Return the saved record as a DTO (now includes the generated EmployeeId)
            return MapToResponseDto(staff);
        }



         // Update an existing administrative staff member's details
        public async Task<AdministrativeStaffResponseDto?> UpdateAsync(int id, UpdateAdministrativeStaffDto dto)
        {
            // First fetch the existing record from the database
            var staff = await _administrativeStaffRepository.GetByIdAsync(id);

            // Return null if the record doesn't exist
            if (staff == null) return null;

            // Overwrite the existing fields with the new values from the DTO
            staff.Title = dto.Title;
            staff.Name = dto.Name;
            staff.NIC = dto.NIC;
            staff.Gender = dto.Gender;
            staff.DateOfBirth = dto.DateOfBirth;
            staff.Address = dto.Address;
            staff.Phone = dto.Phone;
            staff.Email = dto.Email;
            staff.EmploymentStatus = dto.EmploymentStatus;
            staff.EmployeeType = dto.EmployeeType;
            staff.Position = dto.Position;

            // Tell the repository the record has changed, then save
            _administrativeStaffRepository.Update(staff);
            await _administrativeStaffRepository.SaveChangesAsync();

            return MapToResponseDto(staff);
        }



        // Delete a administrative staff record
        public async Task<bool> DeleteAsync(int id)
        {
            var staff = await _administrativeStaffRepository.GetByIdAsync(id);

            // Nothing to delete if the record doesn't exist
            if (staff == null) return false;

            _administrativeStaffRepository.Delete(staff);
            await _administrativeStaffRepository.SaveChangesAsync();

            return true;
        }



        // Converts a raw NonAcademicStaff model into a NonAcademicStaffResponseDto
        private static AdministrativeStaffResponseDto MapToResponseDto(AdministrativeStaff n)
        {
            return new AdministrativeStaffResponseDto
            {
                EmployeeId = n.EmployeeId,
                Title = n.Title.ToString(),
                Name = n.Name,
                NIC = n.NIC,
                Gender = n.Gender.ToString(),
                DateOfBirth = n.DateOfBirth,
                Address = n.Address,
                Phone = n.Phone,
                Email = n.Email,
                DateOfJoining = n.DateOfJoining,
                EmploymentStatus = n.EmploymentStatus.ToString(),
                EmployeeType = n.EmployeeType,
                Position = n.Position
            };
        }
    }
}