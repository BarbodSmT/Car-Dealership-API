using System.ComponentModel.DataAnnotations;

namespace BareProject.Models;

public class ServiceModel
{
    public string Name { get; set; }
    public ICollection<int> RawMaterialId { get; set; }
    public DateTime? Date { get; set; }
    
    public IEnumerable < ValidationResult > Validate(ValidationContext validationContext) {    
        if (Name.Length < 1)
        {
            yield return new ValidationResult("اسم سرویس الزامی است.");
        }

        if (RawMaterialId.Count < 1)
        {
            yield return new ValidationResult("ثبت مواد اولیه الزامی است.");
        }

        if (Date.HasValue)
        {
            yield return new ValidationResult("ثبت تاریخ الزامی است.");
        }
    } 
}