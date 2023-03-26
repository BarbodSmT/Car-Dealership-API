using AutoMapper;
using BareProject.Models.DTO;
using Common.Enums;
using Common.Exceptions;
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
public class ContractController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IRepository<Contract> _repository;
    private readonly IRepository<Car> _carRepository;
    private readonly IUserRepository _userRepository;
    public ContractController(IMapper mapper, IRepository<Contract> repository, IRepository<Car> carRepository, IUserRepository userRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _carRepository = carRepository;
        _userRepository = userRepository;
    }
    [HttpGet]
    [Authorize(Roles = Permission.Manager)]
    public virtual async Task<ApiResult<List<Contract>>> GetAll(CancellationToken cancellationToken)
    {
        var contracts = await _repository.Table
            .Include(s => s.Car)
            .Include(s => s.Customer)
            .Include(s => s.Dealer)
            .ToListAsync(cancellationToken);
        if (!contracts.Any())
            return NotFound();
        return Ok(contracts);
    }

        [HttpGet("{id:int}")]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult<Contract>> Get(int id, CancellationToken cancellationToken)
        {
            var contract = await _repository.Table
                .Include(s => s.Car)
                .Include(s => s.Customer)
                .Include(s => s.Dealer)
                .SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);
            if (contract == null)
                return NotFound();
            return contract;
        }

        [HttpPost]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult<Contract>> Create(ContractDTO contractDto, CancellationToken cancellationToken)
        {
            var contract = _mapper.Map<Contract>(contractDto);
            var iCar = await _carRepository.GetByIdAsync(cancellationToken, contractDto.CarId);
            var iCustomer = await _userRepository.GetByIdAsync(cancellationToken, contract.CustomerId);
            var iDealer = await _userRepository.GetByIdAsync(cancellationToken, contract.DealerId);
            if (iCar == null)
                return BadRequest("ماشین با ایدی مورد نظر یافت نشد.");
            if (iCustomer == null)
                return BadRequest("مشتری با ایدی مورد نظر یافت نشد.");            
            if (iDealer == null)
                return BadRequest("دلال با ایدی مورد نظر یافت نشد.");
            contract.Customer = iCustomer;
            contract.Dealer = iDealer;
            contract.Car = iCar;
            
            await _repository.AddAsync(contract, cancellationToken);
            
            iCar.Owner = iCustomer;
            iCar.Contracts.Add(contract);
            await _carRepository.UpdateAsync(iCar, cancellationToken);
            iCustomer.CustomerContracts.Add(contract);
            iDealer!.DealerContracts.Add(contract);
            await _userRepository.UpdateAsync(iCustomer, cancellationToken);
            await _userRepository.UpdateAsync(iDealer, cancellationToken);
            return Ok(contract);
        }

        [HttpPut]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult> Update(int id, ContractDTO contract, CancellationToken cancellationToken)
        {
            var updateContract = await _repository.GetByIdAsync(cancellationToken, id);
            if (updateContract == null)
                return NotFound();
            var iCar = await _carRepository.GetByIdAsync(cancellationToken, contract.CarId);
            var iCustomer = await _userRepository.GetByIdAsync(cancellationToken, contract.CustomerId);
            var iDealer = await _userRepository.GetByIdAsync(cancellationToken, contract.DealerId);
            if (iCar == null)
                return BadRequest("ماشین با ایدی مورد نظر یافت نشد.");
            if (iCustomer == null)
                return BadRequest("مشتری با ایدی مورد نظر یافت نشد.");
            if (iDealer == null)
                return BadRequest("دلال با ایدی مورد نظر یافت نشد.");
            var rEntity = _mapper.Map<Contract>(contract);
            rEntity.Id = id;
            rEntity.Car = iCar;
            rEntity.Customer = iCustomer;
            rEntity.Dealer = iDealer;
            iCar.Contracts.Add(rEntity);
            iCustomer.CustomerContracts.Add(rEntity);
            iDealer.DealerContracts.Add(rEntity);
            iCar.Owner!.Cars.Remove(iCar);
            iCar.Owner = iCustomer;
            iCustomer.Cars.Add(iCar);
            await _repository.UpdateAsync(rEntity, cancellationToken);
            await _userRepository.UpdateAsync(iDealer, cancellationToken);
            await _userRepository.UpdateAsync(iCustomer, cancellationToken);
            await _carRepository.UpdateAsync(iCar, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var contract = await _repository.GetByIdAsync(cancellationToken, id);
            if (contract == null)
                return NotFound();
            await _repository.DeleteAsync(contract, cancellationToken);

            return Ok();
        }
}