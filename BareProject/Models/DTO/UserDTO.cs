using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using Common.Enums;
using Entities;
using Entities.UserManager;
using WebFramwork.Api;
using WebFramwork.CustomMapping;

namespace BareProject.Models.DTO;

public class UserDTO : BaseDto<UserDTO, User>, IValidatableObject
{
    [Required]
    [StringLength(100)]
    public string UserName { get; set; }

    [Required]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [StringLength(500)]
    public string Password { get; set; }

    [Required]
    [StringLength(100)]
    public string FName { get; set; }
    [Required]
    [StringLength(100)]
    public string LName { get; set; }

    public int Age { get; set; }
    public int PhoneNumber { get; set; }

    public string Gender { get; set; }
    public string Permission { get; set; }
    public string Address { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!Email.Any(c => c == '@'))
            yield return new ValidationResult("ایمیل معتبر نیست", new[] { nameof(UserName) });
        if (!Password.Any(char.IsDigit))
            yield return new ValidationResult("رمز عبور باید دارای عدد باشد", new[] { nameof(Password) });
        if (!Permission.Equals(Common.Enums.Permission.Customer) && !Permission.Equals(Common.Enums.Permission.Dealer) && !Permission.Equals(Common.Enums.Permission.Manager))
            yield return new ValidationResult("سطح دسترسی تعریف نشده", new[] { nameof(Permission) });
    }
}
    