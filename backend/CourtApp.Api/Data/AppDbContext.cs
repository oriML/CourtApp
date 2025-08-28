using CourtApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Added for IConfiguration

namespace CourtApp.Api.Data;

public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration; // Added for configuration access

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<ContactRequest> ContactRequests { get; set; }
    public DbSet<User> Users { get; set; } // Assuming User entity exists and needs to be mapped

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Get the PostgreSQL connection string from configuration
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure ContactRequest entity
        modelBuilder.Entity<ContactRequest>(entity =>
        {
            entity.Property(p => p.Id).ValueGeneratedOnAdd(); // Ensure UUID generation
            entity.Property(p => p.Departments)
                  .HasColumnType("text[]"); // Map Departments to PostgreSQL text array

            // Configure RowVersion for xmin concurrency
            entity.Property(p => p.RowVersion)
                  .HasColumnType("xid") // xmin is of type xid (unsigned int)
                  .HasColumnName("xmin") // Map to the xmin system column
                  .ValueGeneratedOnAddOrUpdate() // xmin is updated on add/update
                  .IsConcurrencyToken(); // Mark as concurrency token

            // Configure CreatedAt and UpdatedAt to timestamptz
            entity.Property(p => p.CreatedAt)
                  .HasColumnType("timestamptz")
                  .HasDefaultValueSql("now()"); // Set default value to current timestamp

            entity.Property(p => p.UpdatedAt)
                  .HasColumnType("timestamptz")
                  .HasDefaultValueSql("now()"); // Set default value to current timestamp
        });

        // Configure User entity (assuming it exists and needs mapping)
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(p => p.Id).ValueGeneratedOnAdd(); // Ensure UUID generation
            // Add other User entity configurations if necessary
        });
    }
}