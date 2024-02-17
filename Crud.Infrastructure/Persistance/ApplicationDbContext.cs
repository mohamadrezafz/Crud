
using Crud.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Crud.Domain.Common;
using Crud.Application.Common.Interfaces;

namespace Crud.Infrastructure.Persistance;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IBaseEventService _baseEventService;

    public ApplicationDbContext(DbContextOptions options, IBaseEventService baseEventService) : base(options)
    {
        this._baseEventService = baseEventService;
    }

    public DbSet<Customer> Customers => Set<Customer>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {

        var events = ChangeTracker.Entries<IHasBaseEvent>().Select(v => v.Entity.BaseEvents).SelectMany(v => v).Where(domainEvent => !domainEvent.IsPublish).ToList();
        var response = await base.SaveChangesAsync(cancellationToken);

        foreach (var item in events)
        {
            item.IsPublish = true;
            await _baseEventService.Publish(item);
        }

        return response;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
