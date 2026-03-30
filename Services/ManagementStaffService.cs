using SchoolManagementSystem.DTOs.ManagementStaff;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Services
{
    // contains all BUSINESS LOGIC for management staff operations
    // calls the repository for data access and returns DTOs to the Controller
    public class ManagementStaffService : IManagementStaffService
    {
        private readonly IManagementStaffRepository _managementStaffRepository;

        public ManagementStaffService(IManagementStaffRepository managementStaffRepository)
        {
            _managementStaffRepository = managementStaffRepository;
        }



        // Get all management staff members and convert each to a response DTO
        public async Task<IEnumerable<ManagementStaffResponseDto>> GetAllAsync()
        {
            // Ask the repository for all raw ManagementStaff model objects
            var staffList = await _managementStaffRepository.GetAllAsync();

            // Convert each ManagementStaff model to a ManagementStaffResponseDto
            return staffList.Select(m => MapToResponseDto(m));
        }



        // Get one management staff member by ID and convert to a response DTO
        public async Task<ManagementStaffResponseDto?> GetByIdAsync(int id)
        {
            var staff = await _managementStaffRepository.GetByIdAsync(id);

            if (staff == null) return null; // Return null if not found

            return MapToResponseDto(staff);
        }



        // Create a new management staff record
        public async Task<ManagementStaffResponseDto> CreateAsync(CreateManagementStaffDto dto)
        {
            // Business rule: each staff member must have a unique NIC number
            bool nicTaken = await _managementStaffRepository.NICExistsAsync(dto.NIC);
            if (nicTaken)
            {
                throw new InvalidOperationException(
                    $"A staff member with NIC '{dto.NIC}' already exists.");
            }

            // Business rule: each staff member must have a unique email address
            bool emailTaken = await _managementStaffRepository.EmailExistsAsync(dto.Email);
            if (emailTaken)
            {
                throw new InvalidOperationException(
                    $"A staff member with email '{dto.Email}' already exists.");
            }

            // Map the incoming DTO fields onto a new ManagementStaff model object
            var staff = new ManagementStaff
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
            await _managementStaffRepository.AddAsync(staff);
            await _managementStaffRepository.SaveChangesAsync();

            return MapToResponseDto(staff); // Return the saved record as a DTO
        }



        // Update an existing management staff member's details
        public async Task<ManagementStaffResponseDto?> UpdateAsync(int id, UpdateManagementStaffDto dto)
        {
            // 1st fetch the existing record from the database
            var staff = await _managementStaffRepository.GetByIdAsync(id);

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
            _managementStaffRepository.Update(staff);
            await _managementStaffRepository.SaveChangesAsync();

            return MapToResponseDto(staff);
        }



        // Delete a management staff record
        public async Task<bool> DeleteAsync(int id)
        {
            var staff = await _managementStaffRepository.GetByIdAsync(id);

            if (staff == null) return false; // Nothing to delete if the record doesn't exist

            _managementStaffRepository.Delete(staff);
            await _managementStaffRepository.SaveChangesAsync();

            return true;
        }




        // Converts a raw ManagementStaff model into a ManagementStaffResponseDto
        private static ManagementStaffResponseDto MapToResponseDto(ManagementStaff m)
        {
            return new ManagementStaffResponseDto
            {
                EmployeeId = m.EmployeeId,
                Title = m.Title.ToString(),
                Name = m.Name,
                NIC = m.NIC,
                Gender = m.Gender.ToString(),
                DateOfBirth = m.DateOfBirth,
                Address = m.Address,
                Phone = m.Phone,
                Email = m.Email,
                DateOfJoining = m.DateOfJoining,
                EmploymentStatus = m.EmploymentStatus.ToString(),
                EmployeeType = m.EmployeeType,
                Position = m.Position
            };
        }
    }
}