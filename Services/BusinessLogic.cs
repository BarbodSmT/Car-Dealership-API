using Data.Contracts;
using Data.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class BusinessLogic : IBusinessLogic
{
    private readonly IRepository<RawMaterial> _mRepository;
    private readonly IRepository<Service> _sRepository;
    public BusinessLogic(IRepository<RawMaterial> mBase, IRepository<Service> sBase)
    {
        _mRepository = mBase;
        _sRepository = sBase;
    }
    public async Task<ICollection<RawMaterial>> BGetRawMaterials()
    {
        return await _mRepository.Entities
            .Include(s => s.Service)
            .ToListAsync();
    }
    public async Task<RawMaterial?> BGetRawMaterial(int id, CancellationToken cn)
    {
        var c = await _mRepository.GetByIdAsync(cn, id);
        if (c != null && c.ServiceId != null)
            c.Service = await _sRepository.GetByIdAsync(cn, c.ServiceId);
        return c;
    }
    public async Task BCreateRawMaterial(RawMaterial input, CancellationToken cn)
    { 
       await _mRepository.AddAsync(input, cn);
    }
    public async Task BUpdateRawMaterial(RawMaterial input, CancellationToken cn)
    {
        await _mRepository.UpdateAsync(input, cn);
    }
    public async Task<bool> BDeleteRawMaterial(int id, CancellationToken cn)
    {
        var c = await _mRepository.GetByIdAsync(cn, id);
        if (c == null) return false;
        await _mRepository.DeleteAsync(c, cn);
        return true;
    }
    public async Task<ICollection<Service>> BGetServices()
    {
        return await _sRepository.Entities
            .Include(s => s.RawMaterials)
            .ToListAsync();
    }
    public async Task<Service?> BGetService(int id, CancellationToken cn)
    {
        var c = await _sRepository.GetByIdAsync(cn, id);

        if (c != null)
        {
            var x = await _mRepository.Entities
                .Where(s => s.ServiceId == c.Id)
                .ToListAsync();
            c.RawMaterials.AddRange(x);
        }

        return c;
    }
    public async Task BCreateService(Service input, CancellationToken cn)
    {
        await _sRepository.AddAsync(input, cn);
    }
    public async Task BUpdateService(Service input, CancellationToken cn)
    {
        await _sRepository.UpdateAsync(input, cn);
    }
    public async Task<bool> BDeleteService(int id, CancellationToken cn)
    {
        var c = await _sRepository.GetByIdAsync(cn, id);
        if (c == null) return false;
        await _sRepository.DeleteAsync(c, cn);
        return true;
    }
}