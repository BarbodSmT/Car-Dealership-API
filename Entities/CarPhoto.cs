namespace Entities;

public class CarPhoto : IEntity<int>
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string publicId { get; set; }
    public Car Car { get; set; }
    public int CarId { get; set; }
    public string Created { get; set; } = " ";
    public string CreatedByUserId { get; set; } = " ";
    public string Modified { get; set; } = " ";
    public string ModifiedByUserId { get; set; } = " ";
}