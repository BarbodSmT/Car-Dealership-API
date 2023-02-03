using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{

    public class RawMaterial : BaseEntity
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public Service? Service { get; set; }
        public int? ServiceId { get; set; }
    }

    public class RawMaterialConfiguration : IEntityTypeConfiguration<RawMaterial>
    {
        public void Configure(EntityTypeBuilder<RawMaterial> builder)
        {
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Price).IsRequired();
            builder
                .HasOne(s => s.Service)
                .WithMany(g => g.RawMaterials)
                .HasForeignKey(s => s.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}