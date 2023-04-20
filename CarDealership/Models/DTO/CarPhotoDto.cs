using Entities;
using WebFramwork.Api;

namespace CarDealership.Models.DTO;

public class CarPhotoDto : BaseDto<CarPhotoDto, CarPhoto>
{
    public string Url { get; set; }
    public string publicId { get; set; }
}