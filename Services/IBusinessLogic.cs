using Entities;

namespace Services;

public interface IBusinessLogic
{
    public Task<ICollection<RawMaterial>> BGetRawMaterials();
    public Task<RawMaterial?> BGetRawMaterial(int id, CancellationToken cn);
    public Task BCreateRawMaterial(RawMaterial input, CancellationToken cn);
    public Task BUpdateRawMaterial(RawMaterial input, CancellationToken cn);
    public Task<bool> BDeleteRawMaterial(int id, CancellationToken cn);
    public Task<ICollection<Service>> BGetServices();
    public Task<Service?> BGetService(int id, CancellationToken cn);
    public Task BCreateService(Service input, CancellationToken cn);
    public Task BUpdateService(Service input, CancellationToken cn);
    public Task<bool> BDeleteService(int id, CancellationToken cn);
}