using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using AutoMapper;
using CarDealership.Models.DTO;
using Common;
using Common.Enums;
using Common.Exceptions;
using Data.Contracts;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services;
using WebFramework.Api;
using WebFramwork.Api;
using Entities.UserManager;
using WebFramwork.Filters;

namespace CarDealership.Controllers.Api.V1;

[ApiVersion("1")]
public class UserController : BaseController
{
        private readonly IUserRepository userRepository;
        private readonly IRepository<Car> carRepository;
        private readonly IRepository<Contract> contractRepository;
        private readonly IJwtService jwtService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger, IJwtService jwtService,
            UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, IMapper mapper, IRepository<Car> carRepository, IRepository<Contract> contractRepository)
        {
            this.userRepository = userRepository;
            _logger = logger;
            this.jwtService = jwtService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            _mapper = mapper;
            this.carRepository = carRepository;
            this.contractRepository = contractRepository;
        }

        [HttpGet]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult<List<User>>> GetAll(CancellationToken cancellationToken)
        {
            //var userName = HttpContext.User.Identity.GetUserName();
            //userName = HttpContext.User.Identity.Name;
            //var userId = HttpContext.User.Identity.GetUserId();
            //var userIdInt = HttpContext.User.Identity.GetUserId<int>();
            //var phone = HttpContext.User.Identity.FindFirstValue(ClaimTypes.MobilePhone);
            //var role = HttpContext.User.Identity.FindFirstValue(ClaimTypes.Role);

            var users = await userRepository.Table
                .Include(s => s.Cars)
                .Include(s => s.CustomerContracts)
                .Include(s => s.DealerContracts)
                .ToListAsync(cancellationToken);
            if (!users.Any())
                return NotFound();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult<User>> Get(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.Table
                .Include(s => s.Cars)
                .Include(s => s.CustomerContracts)
                .Include(s => s.DealerContracts)
                .SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);
            if (user == null)
                return NotFound();

            return user;
        }

        /// <summary>
        /// This method generates a JWT Token
        /// </summary>
        /// <param name="tokenRequest">The information of token request</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [AllowAnonymous]
        public virtual async Task<ActionResult> Token([FromForm] TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            if (!tokenRequest.grant_type.Equals("password", StringComparison.OrdinalIgnoreCase))
                throw new Exception("OAuth flow is not password.");

            //var user = await userRepository.GetByUserAndPass(username, password, cancellationToken);
            var user = await userManager.FindByNameAsync(tokenRequest.username);
            if (user == null)
                throw new Exception("نام کاربری یا رمز عبور اشتباه است");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, tokenRequest.password);
            if (!isPasswordValid)
                throw new Exception("نام کاربری یا رمز عبور اشتباه است");


            //if (user == null)
            //    throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");

            var jwt = await jwtService.GenerateAsync(user);
            return new JsonResult(jwt);
        }

        [HttpPost]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult<User>> Create(UserDTO userDto, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(userDto);
            if (userManager.Users.Any(p => p.UserName == user.UserName)) 
                return BadRequest("نام کاربری تکراری است.");
            if (userManager.Users.Any(p => p.Email == user.Email)) 
                return BadRequest("ایمیل تکراری است.");
            user.CreatedByUserId = userId;
            await userManager.CreateAsync(user, userDto.Password);
            await userManager.AddToRoleAsync(user, userDto.Permission);
            return user;
        }

        [HttpPut]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult> Update(int id, UserDTO user, CancellationToken cancellationToken)
        {
            var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);
            if (updateUser == null)
                return NotFound();
            var rEntity = _mapper.Map<User>(user);
            rEntity.Id = id;
            rEntity.ModifiedByUserId = userId;
            await userManager.UpdateAsync(rEntity);
            var userRoles = await userManager.GetRolesAsync(updateUser);
            await userManager.RemoveFromRolesAsync(updateUser, userRoles);
            await userManager.AddToRoleAsync(rEntity, user.Permission);
            await userRepository.UpdateAsync(updateUser, cancellationToken);
            

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();
            var userRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, userRoles);
            await userRepository.DeleteAsync(user, cancellationToken);

            return Ok();
        }
}