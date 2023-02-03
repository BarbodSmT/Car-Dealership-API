using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities
{
    public class User : IdentityUser<long>, IEntity
    {
        public string FullName { get; set; } = "";
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
            #region props

            builder.Property(c => c.PhoneNumber).HasMaxLength(20);
            builder.Property(c => c.IsActive);

            #endregion

            #region relations

            // builder.HasOne<User>(s => s.User)
            //     .WithMany(g => g.chats)
            //     .HasForeignKey(s => s.UserId);

            #endregion
        }
    }
}