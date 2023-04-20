using System.ComponentModel.DataAnnotations.Schema;
using Entities.UserManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{

    public class Car : IEntity<int>
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Model { get; set; }
        public int ProductionYear { get; set; }
        public string Color { get; set; }
        public float Miles { get; set; }
        public string? Class { get; set; }
        public string? Description { get; set; }
        public string? Extra { get; set; }
        public string? Documents { get; set; }
        public string? BodyStatus { get; set; }
        public string? FrontChassisStatus { get; set; }
        public string? BackChassisStatus { get; set; }
        public string? EngineStatus { get; set; }
        public int InsuranceMonthsLeft { get; set; }
        public string? GearBox { get; set; }
        public float Price { get; set; }
        public User? Owner { get; set; }
        public int OwnerId { get; set; }
        public List<Contract> Contracts { get; set; } = new List<Contract>();
        public List<CarPhoto> Photos { get; set; } = new List<CarPhoto>();

        public string Created { get; set; } = " ";
        public string CreatedByUserId { get; set; } = " ";
        public string Modified { get; set; } = " ";
        public string ModifiedByUserId { get; set; } = " ";
    }

    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.Property(p => p.Company).IsRequired();
            builder.Property(p => p.Model).IsRequired();
            builder.Property(p => p.ProductionYear).IsRequired();
            builder.Property(p => p.Color).IsRequired();
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.Miles).IsRequired();
            builder
                .HasMany(s => s.Contracts)
                .WithOne(g => g.Car)
                .HasForeignKey(s => s.CarId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(s => s.Photos)
                .WithOne(g => g.Car)
                .HasForeignKey(s => s.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}