using System.ComponentModel.DataAnnotations;
using Entities;
using WebFramwork.Api;

namespace BareProject.Models.DTO;

public class CarDTO : BaseDto<CarDTO, Car>
{
    public string Company { get; set; }
    public string Model { get; set; }
    public int ProductionYear { get; set; }
    public string Color { get; set; }
    public float Miles { get; set; }
    public string? Class { get; set; }
    public string? Description { get; set; }
    public string? Extra { get; set; }
    public string? Documents { get; set; }
    public string? BodyStatus { get; set; }
    public string? FrontChassisStatus { get; set; }
    public string? BackChassisStatus { get; set; }
    public string? EngineStatus { get; set; }
    public int InsuranceMonthsLeft { get; set; }
    public string? GearBox { get; set; }
    public float Price { get; set; }
    public int OwnerId { get; set; }
}