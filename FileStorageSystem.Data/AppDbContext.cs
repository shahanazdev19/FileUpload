using FileStorageSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace FileStorageSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
    }
}