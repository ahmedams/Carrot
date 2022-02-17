using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Carrot.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Carrot.Entities
{
    public class Context : DbContext
    {
        #region Entities
        public DbSet<Account> Accounts { get; set; }
        #endregion

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            ApplyEntitiesOperations();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplyEntitiesOperations()
        {
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        ProcessCreationEntries(entry);
                        break;
                    case EntityState.Modified:
                        ProcessModificationAuditedEntries(entry);
                        break;
                }
            }
        }

        private static void ProcessCreationEntries(EntityEntry entry)
        {
            if (entry.Entity is EntityBase @base)
                @base.CreatedDate = DateTime.UtcNow;
        }

        private static void ProcessModificationAuditedEntries(EntityEntry entry)
        {
            if (entry.Entity is EntityBase @base)
                @base.ModifiedDate = DateTime.UtcNow;
            ;
        }
    }
}
