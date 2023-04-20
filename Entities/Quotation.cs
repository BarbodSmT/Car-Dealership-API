namespace Entities;

public class Quotation : IEntity<int>
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public float Price { get; set; }
    public DateTime Date { get; set; }
    public string Created { get; set; } = " ";
    public string CreatedByUserId { get; set; } = " ";
    public string Modified { get; set; } = " ";
    public string ModifiedByUserId { get; set; } = " ";
}