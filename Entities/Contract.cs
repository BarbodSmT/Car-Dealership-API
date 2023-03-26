using System.ComponentModel.DataAnnotations.Schema;
using Entities.UserManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{

    public class Contract : IEntity<int>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Car Car { get; set; }
        public int CarId { get; set; }
        public User Customer { get; set; }
        public int CustomerId { get; set; }
        public User Dealer { get; set; }
        public int DealerId { get; set; }
        public float Price { get; set; }

        public string Created { get; set; } = " ";
        public string CreatedByUserId { get; set; } = " ";
        public string Modified { get; set; } = " ";
        public string ModifiedByUserId { get; set; } = " ";
    }

    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.Property(p => p.Date).IsRequired();
            builder.Property(p => p.Price).IsRequired();
            builder
                .HasOne(s => s.Car)
                .WithMany(g => g.Contracts)
                .HasForeignKey(s => s.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}