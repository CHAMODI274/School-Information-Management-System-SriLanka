using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.DTOs;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Enums;
using SchoolManagementSystem.Services;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(SchoolDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }


    }
}