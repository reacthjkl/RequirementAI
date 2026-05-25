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
    public virtual DbSet<PersonaQualityScore>  PersonaQualityScores { get; set; }
    public virtual DbSet<ScenarioQualityScore>  ScenarioQualityScores { get; set; }
    public virtual DbSet<UserStoryQualityScore>  UserStoryQualityScores { get; set; }
    public virtual DbSet<ProjectRefinementJob> ProjectRefinementJobs { get; set; }
    public virtual DbSet<QualityAnalysisJob> QualityAnalysisJobs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcceptanceCriteria>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Wording).HasMaxLength(1024);
        });
        
        modelBuilder.Entity<EdgeCase>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Preconditions)
                .HasMaxLength(1024);
            
            entity.Property(e => e.TriggerAction)
                .HasMaxLength(1024);
            
            entity.Property(e => e.ExpectedBehavior)
                .HasMaxLength(1024);
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
        });
        
        modelBuilder.Entity<Scenario>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Title)
                .HasMaxLength(1024);
            
            entity.Property(e => e.Content)
                .HasMaxLength(int.MaxValue);
            
            entity.HasMany(e => e.UserStories)
                .WithOne(e => e.Scenario)
                .HasForeignKey(e => e.ScenarioId);
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
        });
        
        modelBuilder.Entity<UserStory>(entity => {  
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Title)
                .HasMaxLength(1024);
            
            entity.Property(e => e.Description)
                .HasMaxLength(int.MaxValue);
            
            entity.HasMany(e => e.AcceptanceCriteria)
                .WithOne(e => e.UserStory)
                .HasForeignKey(e => e.UserStoryId);
            
            entity.HasMany(e => e.EdgeCases)
                .WithOne(e => e.UserStory)
                .HasForeignKey(e => e.UserStoryId);
            
            entity.Property(e => e.Stage)
                .HasMaxLength(255)
                .HasConversion<string>();
        });
        
        modelBuilder.Entity<ProjectRefinementJob>(entity => {  
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(1024);
            
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasConversion<string>();
            
            entity.Property(e => e.StartedAt)
                .HasColumnType("timestamptz");
            
            entity.Property(e => e.FinishedAt)
                .HasColumnType("timestamptz");

            entity.HasOne<Project>()
                .WithMany()
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.CustomInstructions)
                .HasMaxLength(2048);
        });
        
        modelBuilder.Entity<QualityAnalysisJob>(entity => {  
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(1024);
            
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasConversion<string>();
            
            entity.Property(e => e.StartedAt)
                .HasColumnType("timestamptz");
            
            entity.Property(e => e.FinishedAt)
                .HasColumnType("timestamptz");

            entity.HasOne<Project>()
                .WithMany()
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<PersonaQualityScore>(entity => {  
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.Persona)
                .WithMany(e => e.QualityScores)
                .HasForeignKey(e => e.PersonaId);
        });
        
        modelBuilder.Entity<ScenarioQualityScore>(entity => {  
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.Scenario)
                .WithMany(e => e.QualityScores)
                .HasForeignKey(e => e.ScenarioId);
        });
        
        modelBuilder.Entity<UserStoryQualityScore>(entity => {  
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.UserStory)
                .WithMany(e => e.QualityScores)
                .HasForeignKey(e => e.UserStoryId);
        });
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}