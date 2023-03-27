using AutoMapper;
using BareProject.Models.DTO;
using Common.Enums;
using Data.Contracts;
using Data.Repositories;
using Entities;
using Entities.UserManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;
using WebFramwork.Api;

namespace BareProject.Controllers.Api.V1;
[ApiVersion("1")]
public class CarController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IRepository<Car> _repository;
    private readonly IRepository<Contract> _contractRepository;
    private readonly IUserRepository _userRepository;
    public CarController(IMapper mapper, IRepository<Car> repository, IRepository<Contract> contractRepository, IUserRepository userRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _contractRepository = contractRepository;
        _userRepository = userRepository;
    }
    [HttpGet]
    [Authorize(Roles = Permission.Manager +","+ Permission.Dealer)]
    public virtual async Task<ApiResult<List<Car>>> GetAll(CancellationToken cancellationToken)
    {
        var cars = await _repository.Table
            .Include(s => s.Owner)
            .Include(s => s.Contracts)
            .ToListAsync(cancellationToken);
        if (!cars.Any())
            return NotFound();
        return Ok(cars);
    }

        [HttpGet("{id:int}")]
        [Authorize(Roles = Permission.Manager+","+ Permission.Dealer)]
        public virtual async Task<ApiResult<Car>> Get(int id, CancellationToken cancellationToken)
        {
            var car = await _repository.Table
                .Include(s => s.Owner)
                .Include(s => s.Contracts)
                .SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);
            if (car == null)
                return NotFound();
            return car;
        }

        [HttpPost]
        [Authorize(Roles = Permission.Manager+","+ Permission.Dealer)]
        public virtual async Task<ApiResult<Car>> Create(CarDTO carDTO, CancellationToken cancellationToken)
        {
            var car = _mapper.Map<Car>(carDTO);
            var iOwner = await _userRepository.GetByIdAsync(cancellationToken, car.OwnerId);
            if (iOwner == null)
                return BadRequest("صاجب ماشین یافت نشد.");
            car.Owner = iOwner;
            await _repository.AddAsync(car, cancellationToken);
            iOwner.Cars.Add(car);
            await _userRepository.UpdateAsync(iOwner, cancellationToken);
            return Ok(car);
        }

        [HttpPut]
        [Authorize(Roles = Permission.Manager+","+ Permission.Dealer)]
        public virtual async Task<ApiResult> Update(int id, CarDTO carDTO, CancellationToken cancellationToken)
        {
            var updateCar = await _repository.GetByIdAsync(cancellationToken, id);
            if (updateCar == null)
                return NotFound();
            var rEntity = _mapper.Map<Car>(carDTO);
            var iOwner = await _userRepository.GetByIdAsync(cancellationToken, rEntity.OwnerId);
            if (iOwner == null)
                return BadRequest("صاجب ماشین یافت نشد.");
            updateCar.Owner!.Cars.Remove(updateCar);
            rEntity.Owner = iOwner;
            await _repository.UpdateAsync(rEntity, cancellationToken);
            iOwner.Cars.Add(rEntity);
            await _userRepository.UpdateAsync(iOwner, cancellationToken);
            await _userRepository.UpdateAsync(updateCar.Owner, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = Permission.Manager+","+ Permission.Dealer)]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var car = await _repository.GetByIdAsync(cancellationToken, id);
            if (car == null)
                return NotFound();
            await _repository.DeleteAsync(car, cancellationToken);

            return Ok();
        }
}