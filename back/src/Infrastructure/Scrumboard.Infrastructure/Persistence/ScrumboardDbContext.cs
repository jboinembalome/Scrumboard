using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Identity;
using System.Reflection;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Extensions;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Attachments;
using Scrumboard.Domain.Cards.Checklists;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Common;

namespace Scrumboard.Infrastructure.Persistence;

public sealed class ScrumboardDbContext : IdentityDbContext<ApplicationUser>, IPersistedGrantDbContext
{
    private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public ScrumboardDbContext(
        DbContextOptions options, 
        IOptions<OperationalStoreOptions> operationalStoreOptions, 
        ICurrentUserService currentUserService,
        IDateTime dateTime) : base(options)
    {
        _operationalStoreOptions = operationalStoreOptions;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public DbSet<Activity> Activities { get; set; }
    public DbSet<Adherent> Adherents { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<BoardSetting> BoardSettings { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Checklist> Checklists { get; set; }
    public DbSet<ChecklistItem> ChecklistItems { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<ListBoard> ListBoards { get; set; }
    public DbSet<Team> Teams { get; set; }
    
    // IPersistedGrantDbContext
    public DbSet<PersistedGrant> PersistedGrants { get; set; }
    public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }
    public DbSet<Key> Keys { get; set; }
    public DbSet<ServerSideSession> ServerSideSessions { get; set; }
    public DbSet<PushedAuthorizationRequest> PushedAuthorizationRequests { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId ?? string.Empty;
                    entry.Entity.CreatedDate = _dateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId ?? string.Empty;
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
        
        builder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);
    }
}
