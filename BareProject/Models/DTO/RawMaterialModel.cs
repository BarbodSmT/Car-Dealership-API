using System.ComponentModel.DataAnnotations;
using Entities;
using WebFramework.Api;
using WebFramwork.Api;

namespace BareProject.Models.DTO;

public class RawMaterialModel : BaseDto<RawMaterialModel, RawMaterial>
{
    public string Name { get; set; }
    public float Price { get; set; }
    public IEnumerable < ValidationResult > Validate(ValidationContext validationContext) {    
        if (Name.Length < 1)
        {
            yield return new ValidationResult("اسم مواد اولیه الزامی است.");
        }         
        if (Price < 1)
        {
            yield return new ValidationResult("قیمت الزامی است.");
        }
    } 
}