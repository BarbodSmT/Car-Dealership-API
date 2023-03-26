using Common.Utilities;
using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Entities.UserManager;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    //DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

                var entitiesAssembly = typeof(IEntity).Assembly;

                modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
                modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
                modelBuilder.AddRestrictDeleteBehaviorConvention();
                modelBuilder.AddSequentialGuidForIdConvention();
                modelBuilder.AddPluralizingTableNameConvention();
        }

        public override int SaveChanges()
        {
            _cleanString();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _cleanString();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void _cleanString()
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            var now = DateTime.UtcNow;
            PersianCalendar pc = new();
            string PDate = pc.GetYear(DateTime.Now)
                .ToString("00") + "/" + pc.GetMonth(DateTime.Now).ToString("00") + "/" + pc.GetDayOfMonth(DateTime.Now).ToString("00");

            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;
                //trucker
                if (item.Entity is IEntity entity)
                {

                    switch (item.State)
                    {
                        case EntityState.Added:
                            entity.Created = PDate;
                            entity.Modified = PDate;
                            break;
                        case EntityState.Modified:
                            Entry(entity).Property(x => x.Created).IsModified = false;
                            entity.Modified = PDate;
                            break;
                    }
                }

                //clean string
                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    var propName = property.Name;
                    var val = (string)property.GetValue(item.Entity, null);

                    if (val.HasValue())
                    {
                        var newVal = val.Fa2En().FixPersianChars();
                        if (newVal == val)
                            continue;
                        property.SetValue(item.Entity, newVal, null);
                    }
                }
            }
        }
    }
}
