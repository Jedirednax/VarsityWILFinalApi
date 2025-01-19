using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VCTicketTrackerAPIClassLibrary;
using VCTicketTrackerAPIClassLibrary.Auth;
using VCTicketTrackerAPIClassLibrary.StorageDB;
using VCTicketTrackerAPIClassLibrary.Tickets;
using VCTicketTrackerAPIClassLibrary.UserClasses.Dto;
using VCTicketTrackerAPIClassLibrary.UserFactoryClasses;

namespace VarsityWILFinalApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AATestController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;

        public AATestController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ILogger<UsersController> logger)
        {

            _jwtKey = configuration["Jwt:Key"];
            _jwtIssuer = configuration["Jwt:Issuer"];
            _jwtAudience = configuration["Jwt:Audience"];

            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            // Define custom claims

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "Removed FirebaseAdmin User"),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim("role", user.UsersTypes.ToString()),
                new Claim("uid", user.Email),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _jwtIssuer,
            audience: _jwtAudience,

            claims: claims,
            expires: DateTime.Now.AddDays(1), // Set Token expiration
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Get api/<UsersController>
        [HttpGet("GetAllUsers")]
        //[Authorize(Roles = "ServerAdmin, Administrator,SupportMember")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            try
            {


                Console.WriteLine("All Users");
                var foundUsers = DatabaseAccesUtil.GetUser();

                if(foundUsers != null)
                {
                    return foundUsers;
                }
                else
                {
                    return NoContent();
                }
            }
            catch(Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        // POST: api/<UsersController>
        [HttpPost("Register/MultipleStaff")]
        [AllowAnonymous]
        public async Task<IActionResult> MultipleStaff([FromBody] List<StaffDto> userDtos, CancellationToken cancellationToken)
        {
            Debug.WriteLine("");

            // Loop through each userDto and create user
            foreach(var userDto in userDtos)
            {
                Debug.WriteLine(userDto.UserName);
            }

            // Validate the input for all users
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach(var userDto in userDtos)
            {

                UserClient t = new UserClient(userDto);
                if(t != null)
                {

                    var result = await _userManager.CreateAsync(t.Get(), userDto.PasswordHash);

                    if(result.Succeeded)
                    {
                        UserRecordArgs args = new UserRecordArgs()
                    {
                            Uid = userDto.Email,
                            Email = userDto.Email,
                            Password = UserHandler._PasswordHasher(userDto.PasswordHash, userDto.Email)
                        };

                        // UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
                    }
                    else
                    {
                        // Return detailed error messages for the failed user creation attempt
                        return BadRequest(result.Errors.Select(e => e.Description));
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }

            return Ok(new { Message = "Users created successfully" });
        }
        // POST: api/<UsersController>
        [HttpPost("Register/MultipleStudent")]
        [AllowAnonymous]
        public async Task<IActionResult> MultipleStudent([FromBody] List<StudentDto> userDtos, CancellationToken cancellationToken)
        {
            Debug.WriteLine("");

            // Loop through each userDto and create user
            foreach(var userDto in userDtos)
            {
                Debug.WriteLine(userDto.UserName);
            }

            // Validate the input for all users
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach(var userDto in userDtos)
            {

                UserClient t = new UserClient(userDto);
                if(t != null)
                {

                    var result = await _userManager.CreateAsync(t.Get(), userDto.PasswordHash);

                    if(result.Succeeded)
                    {
                        UserRecordArgs args = new UserRecordArgs()
                    {
                            Uid = userDto.Email,
                            Email = userDto.Email,
                            Password = UserHandler._PasswordHasher(userDto.PasswordHash, userDto.Email)
                        };

                        // UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
                    }
                    else
                    {
                        // Return detailed error messages for the failed user creation attempt
                        return BadRequest(result.Errors.Select(e => e.Description));
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }

            return Ok(new { Message = "Users created successfully" });
        }

        // POST api/<TicketController>
        [Route("LoadMany")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddTickets([FromBody] List<TicketCreateDto> ticketDTO)
        {
            try
            {
                foreach(var ticketd in ticketDTO)
                {
                    var ticket = new Ticket(ticketd);
                    if(ticket.Insert() >0)
                    {
                        return Problem("Failded to create ticket");
                    }
                }
                return Ok("Ticket Created Succsefuly");
            }

            catch(Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

    }
}