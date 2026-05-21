using Microsoft.EntityFrameworkCore;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence;

public class RequirementAIContext(DbContextOptions<RequirementAIContext> options) : DbContext(options)
{
    public virtual DbSet<AcceptanceCriteria> AcceptanceCriteria { get; set; }
    public virtual DbSet<EdgeCase> EdgeCases { get; set; }
    public virtual DbSet<Organization> Organizations { get; set; }
    public virtual DbSet<Persona> Personas { get; set; }
    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<Scenario> Scenarios { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserStory> UserStories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcceptanceCriteria>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Wording).HasMaxLength(1028);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            
        });
        
        modelBuilder.Entity<EdgeCase>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Preconditions)
                .HasMaxLength(1028);
            
            entity.Property(e => e.TriggerAction)
                .HasMaxLength(1028);
            
            entity.Property(e => e.ExpectedBehavior)
                .HasMaxLength(1028);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            
        });
        
        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(255);

            entity.HasMany(e => e.Users)
                .WithOne(e => e.Organization)
                .HasForeignKey(e => e.OrganizationId);
            
            entity.HasMany(e => e.Projects)
                .WithOne(e => e.Organization)
                .HasForeignKey(e => e.OrganizationId);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            
        });
        
        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(255);
            
            entity.Property(e => e.Description)
                .HasMaxLength(2048);
            
            entity.Property(e => e.ContextOfUse)
                .HasMaxLength(2048);
            
            entity.Property(e => e.Goals)
                .HasMaxLength(2048);
            
            entity.Property(e => e.Frustrations)
                .HasMaxLength(2048);
            
            entity.HasMany(e => e.Scenarios)
                .WithOne(e => e.Persona)
                .HasForeignKey(e => e.PersonaId);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            
        });
        
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .HasMaxLength(255);
            
            entity.Property(e => e.Description)
                .HasMaxLength(2048);
            
            entity.HasMany(e => e.Personas)
                .WithOne(e => e.Project)
                .HasForeignKey(e => e.ProjectId);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            
        });
        
        modelBuilder.Entity<Scenario>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Title)
                .HasMaxLength(1028);
            
            entity.Property(e => e.Content)
                .HasMaxLength(int.MaxValue);
            
            entity.HasMany(e => e.UserStories)
                .WithOne(e => e.Scenario)
                .HasForeignKey(e => e.ScenarioId);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            
        });
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(e => e.Email)
                .IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Password)
                .HasMaxLength(255);

            entity.Property(e => e.RefreshToken)
                .HasMaxLength(255);

            entity.Property(e => e.RefreshTokenExpiry);

            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            
        });
        
        modelBuilder.Entity<UserStory>(entity => {  
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Title)
                .HasMaxLength(1028);
            
            entity.Property(e => e.Description)
                .HasMaxLength(int.MaxValue);
            
            entity.HasMany(e => e.AcceptanceCriteria)
                .WithOne(e => e.UserStory)
                .HasForeignKey(e => e.UserStoryId);
            
            entity.HasMany(e => e.EdgeCases)
                .WithOne(e => e.UserStory)
                .HasForeignKey(e => e.UserStoryId);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
          
        });
    }
}