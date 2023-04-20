using System.ComponentModel.DataAnnotations;
using Entities;
using WebFramwork.Api;

namespace CarDealership.Models.DTO;

public class QuotationDTO : BaseDto<QuotationDTO, Quotation>
{
    public string Description { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public float Price { get; set; }
    public DateTime Date { get; set; }
}