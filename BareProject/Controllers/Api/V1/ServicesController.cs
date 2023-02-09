using AutoMapper;
using BareProject.Models;
using BareProject.Models.DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using WebFramework.Api;
using WebFramwork.Api;

namespace BareProject.Controllers.Api.V1;

[ApiVersion("1.0")]
public class ServicesController : BaseController
{
    private readonly IBusinessLogic _iLogic;
    private readonly IMapper _mapper;
    public ServicesController(IBusinessLogic logic, IMapper mapper)
    {
        _iLogic = logic;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult<ICollection<RawMaterial>>> GetServices()
    {
        var items = await _iLogic.BGetServices();
        if (!items.Any())
            return BadRequest("هیچ سرویسی یافت نشد.");
        return Ok(items);
    }

    [HttpGet]
    public async Task<ActionResult<Service>> GetService(int id, CancellationToken cn)
    {
        return await _iLogic.BGetService(id, cn) == null ? BadRequest("سرویس یافت نشد.") : await _iLogic.BGetService(id, cn);
    }

    [HttpPost]
    public async Task<IActionResult> CreateService(ServiceModel input, CancellationToken cn)
    {
        if (!ModelState.IsValid) return BadRequest();
        float servicePrice = 0;
        var serviceEntity = _mapper.Map<Service>(input);
        foreach (var x in input.RawMaterialId)
        {
            var rMaterial = await _iLogic.BGetRawMaterial(x, cn);
            if (rMaterial == null) return BadRequest("مواد اولیه با آیدی " + x + "وجود ندارد.");;
            serviceEntity.RawMaterials.Add(rMaterial);
            rMaterial.Service = serviceEntity;
            rMaterial.ServiceId = serviceEntity.Id;
            servicePrice += rMaterial.Price;
        }

        serviceEntity.Price = servicePrice;
        await _iLogic.BCreateService(serviceEntity, cn);
        return Ok("سرویس با موفقیت اضافه شد. قیمت نهایی : " + servicePrice);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateService(int id, ServiceModel input, CancellationToken cn)
    {
        if (!ModelState.IsValid) return BadRequest();
        var serviceEntity = await _iLogic.BGetService(id, cn);
        if (serviceEntity == null) return BadRequest("آیدی سرویس اشتباه است.");
        float servicePrice = 0;
        foreach (var x in input.RawMaterialId)
        {
            var rMaterial = await _iLogic.BGetRawMaterial(x, cn);
            if (rMaterial == null) return BadRequest("مواد اولیه با آیدی " + x + "وجود ندارد.");;
            serviceEntity.RawMaterials.Add(rMaterial);
            rMaterial.Service = serviceEntity;
            rMaterial.ServiceId = serviceEntity.Id;
            servicePrice += rMaterial.Price;
        }
        serviceEntity.Date = input.Date;
        serviceEntity.Name = input.Name;
        serviceEntity.Price = servicePrice;
        await _iLogic.BUpdateService(serviceEntity, cn);
        return Ok("سرویس با موفقیت تغییر داده شد. قیمت نهایی : " + servicePrice);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteService(int id, CancellationToken cn)
    {
        return await _iLogic.BDeleteService(id, cn) == true ? Ok("سرویس با موفقت حذف شد.") : BadRequest("سرویس یافت نشد.");
    }
}