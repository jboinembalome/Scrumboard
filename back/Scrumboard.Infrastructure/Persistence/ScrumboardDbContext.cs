using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Identity;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Checklists;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Common;

namespace Scrumboard.Infrastructure.Persistence;

public class ScrumboardDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public ScrumboardDbContext(
        DbContextOptions options,
        ICurrentUserService currentUserService,
        IDateTime dateTime) : base(options)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public DbSet<Activity> Activities { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<BoardSetting> BoardSettings { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Checklist> Checklists { get; set; }
    public DbSet<ChecklistItem> ChecklistItems { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<ListBoard> ListBoards { get; set; }
    public DbSet<Team> Teams { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.CreatedDate = _dateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModifiedDate = _dateTime.Now;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
