using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{

    public class Service : BaseEntity
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public List<RawMaterial> RawMaterials { get; set; } = new List<RawMaterial>();
        public DateTime? Date { get; set; }
    }

    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.Date).IsRequired();
        }
    }
}