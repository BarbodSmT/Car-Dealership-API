using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Permissions;
using AutoMapper;
using BareProject.Models.DTO;
using Common.Enums;
using Common.Exceptions;
using Data.Repositories;
using Entities;
using Entities.UserManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services;
using WebFramework.Api;
using WebFramwork.Api;

namespace BareProject.Controllers.Api.V2;

[ApiVersion("2")]
public class UserController : BaseController
{
        private readonly IUserRepository userRepository;
        private readonly ILogger<UserController> logger;
        private readonly IJwtService jwtService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger, IJwtService jwtService,
            UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.logger = logger;
            this.jwtService = jwtService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ActionResult<List<User>>> GetAll(CancellationToken cancellationToken)
        {
            //var userName = HttpContext.User.Identity.GetUserName();
            //userName = HttpContext.User.Identity.Name;
            //var userId = HttpContext.User.Identity.GetUserId();
            //var userIdInt = HttpContext.User.Identity.GetUserId<int>();
            //var phone = HttpContext.User.Identity.FindFirstValue(ClaimTypes.MobilePhone);
            //var role = HttpContext.User.Identity.FindFirstValue(ClaimTypes.Role);

            var users = await userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult<User>> Get(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();

            await userManager.UpdateSecurityStampAsync(user);
            //await userRepository.UpdateSecurityStampAsync(user, cancellationToken);

            return user;
        }

        /// <summary>
        /// This method generate JWT Token
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
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, tokenRequest.password);
            if (!isPasswordValid)
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");


            //if (user == null)
            //    throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");

            var jwt = await jwtService.GenerateAsync(user);
            return new JsonResult(jwt);
        }

        [HttpPost]
        [AllowAnonymous]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult<User>> Create(UserDTO userDto, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(userDto);
            var result = await userManager.CreateAsync(user, userDto.Password);

            var result2 = await roleManager.CreateAsync(new Role
            {
                Name = userDto.Permission
            });

            var result3 = await userManager.AddToRoleAsync(user, userDto.Permission);

            //await userRepository.AddAsync(user, userDto.Password, cancellationToken);
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
            

            var result2 = await roleManager.CreateAsync(new Role
            {
                Name = user.Permission
            });
            var userRoles = await userManager.GetRolesAsync(updateUser);
            await userManager.RemoveFromRolesAsync(updateUser, userRoles);
            var result3 = await userManager.AddToRoleAsync(updateUser, user.Permission);
            await userRepository.UpdateAsync(rEntity, cancellationToken);

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = Permission.Manager)]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();
            await userRepository.DeleteAsync(user, cancellationToken);

            return Ok();
        }
}