using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.UserManager
{
    public class User : IdentityUser<int>, IEntity<int>
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public List<Car> Cars { get; set; } = new List<Car>();
        public List<Contract> CustomerContracts { get; set; } = new List<Contract>();
        public List<Contract> DealerContracts { get; set; } = new List<Contract>();
        public bool IsActive { get; set; } = true;
        public DateTimeOffset? ActivatedTime { get; set; }
        public DateTimeOffset? LastLoginDate { get; set; }
        public string Created { get; set; } = " ";
        public string CreatedByUserId { get; set; } = " ";
        public string Modified { get; set; } = " ";
        public string ModifiedByUserId { get; set; } = " ";

    }


    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasMany(s => s.Cars)
                .WithOne(g => g.Owner)
                .HasForeignKey(s => s.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);            
            builder
                .HasMany(s => s.DealerContracts)
                .WithOne(g => g.Dealer)
                .HasForeignKey(s => s.DealerId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(s => s.CustomerContracts)
                .WithOne(g => g.Customer)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}