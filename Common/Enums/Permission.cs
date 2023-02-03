using System.ComponentModel.DataAnnotations;

namespace Common.Enums;

public enum Permission
{
    [Display(Name = "تست اول")] TestSajad = 1,
    [Display(Name = "تست دوم")] TestSajadTwo = 2,
    [Display(Name = "نمایش تست ها")] SeeTests = 3
}