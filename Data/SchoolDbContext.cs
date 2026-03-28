using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Enums;

namespace SchoolManagementSystem.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
            
        }

        // -------------------------DbSets----------------------------
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<ManagementStaff> ManagementStaffs { get; set; }
        public DbSet<AdministrativeStaff> AdministrativeStaffs { get; set; }
        
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ClassCurriculum> ClassCurriculums { get; set; }
        public DbSet<SubjectAllocation> SubjectAllocations { get; set; }
        
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        
        public DbSet<Examination> Examinations { get; set; }
        public DbSet<ExaminationClass> ExaminationClasses { get; set; }
        public DbSet<ExamMark> ExamMarks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            //--------------- 1. Restrict all cascading deletes globally---------------
            // Prevents MySQL circular cascade errors
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            


            //---------------2. Enum-to-string conversions-----------------------------
            // Stores readeble strings insted of integers

            // User
            modelBuilder.Entity<User>()
                .Property(u => u.Role).HasConversion<string>();
            modelBuilder.Entity<User>()
                .Property(u => u.Status).HasConversion<string>();

            // Student
            modelBuilder.Entity<Student>()
                .Property(s => s.Gender).HasConversion<string>();
            modelBuilder.Entity<Student>()
                .Property(s => s.CurrentStatus).HasConversion<string>();

            // Parent
            modelBuilder.Entity<Parent>()
                .Property(p => p.Title).HasConversion<string>();

            // Teacher
            modelBuilder.Entity<Teacher>()
                .Property(t => t.Title).HasConversion<string>();
            modelBuilder.Entity<Teacher>()
                .Property(t => t.Gender).HasConversion<string>();
            modelBuilder.Entity<Teacher>()
                .Property(t => t.EmploymentStatus).HasConversion<string>();

            // ManagementStaff
            modelBuilder.Entity<ManagementStaff>()
                .Property(m => m.Title).HasConversion<string>();
            modelBuilder.Entity<ManagementStaff>()
                .Property(m => m.Gender).HasConversion<string>();
            modelBuilder.Entity<ManagementStaff>()
                .Property(m => m.EmploymentStatus).HasConversion<string>();

            // NonAcademicStaff
            modelBuilder.Entity<AdministrativeStaff>()
                .Property(n => n.Title).HasConversion<string>();
            modelBuilder.Entity<AdministrativeStaff>()
                .Property(n => n.Gender).HasConversion<string>();
            modelBuilder.Entity<AdministrativeStaff>()
                .Property(n => n.EmploymentStatus).HasConversion<string>();

            // Class
            modelBuilder.Entity<Class>()
                .Property(c => c.Medium).HasConversion<string>();

            // Enrollment
            modelBuilder.Entity<Enrollment>()
                .Property(e => e.Status).HasConversion<string>();

            // Attendance
            modelBuilder.Entity<Attendance>()
                .Property(a => a.Status).HasConversion<string>();

            // Examination
            modelBuilder.Entity<Examination>()
                .Property(e => e.ExamType).HasConversion<string>();

            // ExamMark
            modelBuilder.Entity<ExamMark>()
                .Property(em => em.Grade).HasConversion<string>();
            


            //---------------3. Unique indexes-----------------------------------------
            
            // User — unique username and email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();

            // Student — unique admission number
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.AdmissionNumber).IsUnique();

            // Subject — unique subject code
            modelBuilder.Entity<Subject>()
                .HasIndex(s => s.SubjectCode).IsUnique();

            // AcademicYear — unique year label (e.g. "2024")
            modelBuilder.Entity<AcademicYear>()
                .HasIndex(a => a.Year).IsUnique();

            // Enrollment — a student can only enroll in one class per year
            modelBuilder.Entity<Enrollment>()
                .HasIndex(e => new { e.StudentId, e.YearId }).IsUnique();

            // ClassCurriculum — a subject can only appear once per class per year
            modelBuilder.Entity<ClassCurriculum>()
                .HasIndex(cc => new { cc.ClassId, cc.SubjectId, cc.YearId }).IsUnique();

            // SubjectAllocation — a teacher can only be allocated once per curriculum entry
            modelBuilder.Entity<SubjectAllocation>()
                .HasIndex(sa => new { sa.ClassCurriculumId, sa.TeacherId }).IsUnique();

            // Attendance — one record per student per day
            modelBuilder.Entity<Attendance>()
                .HasIndex(a => new { a.EnrollmentId, a.Date }).IsUnique();

            // ExaminationClass — an exam can only be scheduled once per class
            modelBuilder.Entity<ExaminationClass>()
                .HasIndex(ec => new { ec.ExamId, ec.ClassId }).IsUnique();

            // ExamMark — one mark per student per subject per exam
            modelBuilder.Entity<ExamMark>()
                .HasIndex(em => new { em.EnrollmentId, em.ClassCurriculumId, em.ExamId }).IsUnique();



        }
    }
}