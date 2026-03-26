using SchoolManagementSystem.DTOs.Parent;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Services
{
    // contains all BUSINESS LOGIC for parent operations
    // It calls the repository for data access and returns DTOs to the Controller.
    public class ParentService : IParentService
    {
        private readonly IParentRepository _parentRepository;

        // Constructor injection
        public ParentService(IParentRepository parentRepository)
        {
            _parentRepository = parentRepository;
        }


        // Get all parents and convert each to a response DTO
         public async Task<IEnumerable<ParentResponseDto>> GetAllAsync()
        {
            var parents = await _parentRepository.GetAllAsync(); // Ask repository for all raw Parent model objects
            return parents.Select(p => MapToResponseDto(p)); // Convert each Parent model to a ParentResponseDto
        }

        // Get one parent by ID and convert to response DTO
        public async Task<ParentResponseDto?> GetByIdAsync(int id)
        {
            var parent = await _parentRepository.GetByIdAsync(id);
            if (parent == null) return null; // Return null if not found
            return MapToResponseDto(parent);
        }


        // 1. Create a brand new parent record
        public async Task<ParentResponseDto> CreateAsync(CreateParentDto dto)
        {
            // Map the incoming DTO fields onto a new Parent model object
            var parent = new Parent 
            {
                Title = dto.Title,
                Name = dto.Name,
                NIC = dto.NIC,
                Phone = dto.Phone,
                Address = dto.Address,
                Occupation = dto.Occupation,
                WorkPlace = dto.WorkPlace,
                WorkPhone = dto.WorkPhone,
                EmergencyContact = dto.EmergencyContact
                // Note: UserId is not set here.
            };

            await _parentRepository.AddAsync(parent); // Ask the repository to add this parent to the database

            await _parentRepository.SaveChangesAsync(); 
            // After SaveChangesAsync(), EF Core fills in the ParentId automatically

            return MapToResponseDto(parent); // Return the saved parent as a DTO
        }


        // 2. Update an existing parent's details
        public async Task<ParentResponseDto?> UpdateAsync(int id, UpdateParentDto dto)
        {
            var parent = await _parentRepository.GetByIdAsync(id);

            if (parent == null) return null;

            parent.Title = dto.Title;
            parent.Name = dto.Name;
            parent.NIC = dto.NIC;
            parent.Phone = dto.Phone;
            parent.Address = dto.Address;
            parent.Occupation = dto.Occupation;
            parent.WorkPlace = dto.WorkPlace;
            parent.WorkPhone = dto.WorkPhone;
            parent.EmergencyContact = dto.EmergencyContact;

            _parentRepository.Update(parent);
            await _parentRepository.SaveChangesAsync();

            return MapToResponseDto(parent);
        }


        // 3. Delete a parent permanently
        public async Task<bool> DeleteAsync(int id)
        {
            var parent = await _parentRepository.GetByIdAsync(id);

            // Nothing to delete if the parent doesn't exist
            if (parent == null) return false;

            _parentRepository.Delete(parent);
            await _parentRepository.SaveChangesAsync();

            return true;
        }


        // Converts a raw Parent model into a ParentResponseDto to send to the frontend.
        private static ParentResponseDto MapToResponseDto(Parent p)
        {
            return new ParentResponseDto
            {
                ParentId = p.ParentId,
                Title = p.Title.ToString(),  // enum as a string 
                Name = p.Name,
                NIC = p.NIC,
                Phone = p.Phone,
                Address = p.Address,
                Occupation = p.Occupation,
                WorkPlace = p.WorkPlace,
                WorkPhone = p.WorkPhone,
                EmergencyContact = p.EmergencyContact
            };
        }
    }
}