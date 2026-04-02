using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.DTOs.Promotion;
using SchoolManagementSystem.Interfaces;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly SchoolDbContext _context;

        public PromotionService(SchoolDbContext context)
        {
            _context = context;
        }

      
    }
}