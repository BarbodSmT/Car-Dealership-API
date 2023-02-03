using System.ComponentModel.DataAnnotations;

namespace BareProject.Models;

public class RawMaterialModel
{
    public string Name { get; set; }
    public float Price { get; set; }
    public IEnumerable < ValidationResult > Validate(ValidationContext validationContext) {    
        if (Name.Length < 1)
        {
            yield return new ValidationResult("اسم سرویس الزامی است.");
        }         
        if (Price < 1)
        {
            yield return new ValidationResult("قیمت الزامی است.");
        }
    } 
}