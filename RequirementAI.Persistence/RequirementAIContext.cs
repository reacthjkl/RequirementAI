using Microsoft.EntityFrameworkCore;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence;

public class RequirementAIContext(DbContextOptions<RequirementAIContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(e => e.Email)
                .IsUnique();

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(255);

                entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Password)
                .HasMaxLength(255);

            entity.Property(e => e.Provider)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(e => e.ProviderId)
                .HasMaxLength(255);

            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(500);

            entity.Property(e => e.RefreshToken)
                .HasMaxLength(255);

            entity.Property(e => e.RefreshTokenExpiry);

            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
        });
    }
}