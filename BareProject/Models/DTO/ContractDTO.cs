using System.ComponentModel.DataAnnotations;
using Common.Enums;
using Entities;
using WebFramwork.Api;

namespace BareProject.Models.DTO;

public class ContractDTO : BaseDto<ContractDTO, Contract>
{
    public DateTime Date { get; set; }
    public int CarId { get; set; }
    public int CustomerId { get; set; }
    public int DealerId { get; set; }
    public float Price { get; set; }
}