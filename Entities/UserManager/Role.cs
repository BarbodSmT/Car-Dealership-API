using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Role : IdentityRole<long>
    {
        [StringLength(100)]
        public string? Description { get; set; }
    }
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(p => p.Name).IsRequired();
        }
    }
}
