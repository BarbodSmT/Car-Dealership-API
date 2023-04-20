using AutoMapper;
using CarDealership.Models.DTO;
using Common.Enums;
using Data.Contracts;
using Data.Repositories;
using Entities;
using Entities.UserManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.PhotoService;
using WebFramework.Api;
using WebFramwork.Api;

namespace CarDealership.Controllers.Api.V1;
[ApiVersion("1")]
public class CarController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IRepository<Car> _repository;
    private readonly IUserRepository _userRepository;
    private readonly IPhotoService _photoService;
    public CarController(IMapper mapper, IRepository<Car> repository, IRepository<Contract> contractRepository, IUserRepository userRepository, IPhotoService photoService)
    {
        _mapper = mapper;
        _repository = repository;
        _userRepository = userRepository;
        _photoService = photoService;
    }
    [HttpGet]
    [Authorize(Roles = Permission.Manager +","+ Permission.Dealer)]
    public virtual async Task<ApiResult<List<Car>>> GetAll(CancellationToken cancellationToken)
    {
        var cars = await _repository.Table
            .Include(s => s.Owner)
            .Include(s => s.Contracts)
            .Include(s => s.Photos)
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
                .Include(s => s.Photos)
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
            car.CreatedByUserId = userId;
            await _repository.AddAsync(car, cancellationToken);
            iOwner.Cars.Add(car);
            await _userRepository.UpdateAsync(iOwner, cancellationToken);
            return Ok(car);
        }

        [HttpPut]
        [Authorize(Roles = Permission.Manager+","+ Permission.Dealer)]
        public virtual async Task<ApiResult> Update(int id, CarDTO carDTO, CancellationToken cancellationToken)
        {
            var updateCar = await _repository.Table
                .Include(s => s.Owner)
                .Include(s => s.Contracts)
                .Include(s => s.Photos)
                .SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);
            if (updateCar == null)
                return NotFound();
            var rEntity = _mapper.Map<Car>(carDTO);
            var iOwner = await _userRepository.GetByIdAsync(cancellationToken, rEntity.OwnerId);
            if (iOwner == null)
                return BadRequest("صاجب ماشین یافت نشد.");
            updateCar.Owner!.Cars.Remove(updateCar);
            rEntity.Owner = iOwner;
            rEntity.ModifiedByUserId = userId;
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
        /// <summary>
        /// Add a photo to a car
        /// </summary>
        /// <param name="carid">The Id of the car</param>
        /// <param name="PhotoFile">The photo of the car</param>
        /// <param name="cancellationToken"></param>
        /// <returns>returns photo entity</returns>
        [HttpPost("AddPhoto")]
        [Authorize(Roles = Permission.Manager + "," + Permission.Dealer)]
        public virtual async Task<ApiResult<CarPhotoDto>> AddPhoto(int carid, IFormFile PhotoFile, CancellationToken cancellationToken)
        {
            var car = await _repository.Table
                .Include(s => s.Photos)
                .SingleOrDefaultAsync(p => p.Id.Equals(carid), cancellationToken);
            if (car == null)
                return NotFound();
            var result = await _photoService.AddPhotoAsync(PhotoFile);
            if (result.Error != null)
                return BadRequest(result.Error.Message);
            var photo = new CarPhoto
            {
                Url = result.SecureUrl.AbsoluteUri,
                publicId = result.PublicId
            };
            photo.CreatedByUserId = userId;
            car.Photos.Add(photo);
            await _repository.UpdateAsync(car, cancellationToken);
            return _mapper.Map<CarPhotoDto>(photo);
        }
        /// <summary>
        /// Delete a photo of a car
        /// </summary>
        /// <param name="carid">The Id of the car</param>
        /// <param name="photoId">The Id of the photo</param>
        /// <param name="cancellationToken"></param>
        /// <returns>return</returns>
        [HttpDelete("DeletePhoto")]
        [Authorize(Roles = Permission.Manager + "," + Permission.Dealer)]
        public virtual async Task<ApiResult> DeletePhoto(int carid, int photoId, CancellationToken cancellationToken)
        {
            var car = await _repository.Table
                .Include(s => s.Photos)
                .SingleOrDefaultAsync(p => p.Id.Equals(carid), cancellationToken);
            if(car == null)
                return NotFound();
            var photo = car.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null)
                return NotFound();
            if (photo.publicId.Length > 0)
            {
                var result = await _photoService.DeletePhotoAsync(photo.publicId);
                if (result.Error != null)
                    return BadRequest(result.Error.Message);
            }
            car.Photos.Remove(photo);
            await _repository.UpdateAsync(car, cancellationToken);
            return Ok();
        }
}