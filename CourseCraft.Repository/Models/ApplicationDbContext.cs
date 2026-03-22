using Microsoft.EntityFrameworkCore;

namespace CourseCraft.Repository.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseStudentMapping> CourseStudentMappings { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=CourseCraft;Username=postgres;password=YOUR_PASSWORD");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CourseStudentMapping>()
            .HasKey(csm => csm.CourseStudentMappingId);

        modelBuilder.Entity<CourseStudentMapping>()
            .HasOne(csm => csm.Course)
            .WithMany(c => c.CourseStudentMappings)
            .HasForeignKey(csm => csm.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CourseStudentMapping>()
            .HasOne(csm => csm.User)
            .WithMany(u => u.CourseStudentMappings)
            .HasForeignKey(csm => csm.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
