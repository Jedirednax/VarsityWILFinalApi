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
using VCTicketTrackerAPIClassLibrary.Orginisation;
using VCTicketTrackerAPIClassLibrary.StorageDB;
using VCTicketTrackerAPIClassLibrary.UserClasses;
using VCTicketTrackerAPIClassLibrary.UserClasses.Dto;
using VCTicketTrackerAPIClassLibrary.UserFactoryClasses;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
//using static VCTicketTrackerWebAPI.Controllers.UsersController.DepartmentConverter;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//https://localhost:7226/swagger/v1/swagger.json

namespace VarsityWILFinalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;

        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ILogger<UsersController> logger)
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

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "Removed Firebase Service Account"),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim("role", user.UsersTypes.ToString()),
                new Claim("uid", user.Email),

            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
            issuer: _jwtIssuer,
            audience: _jwtAudience,

            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Get api/<UsersController>
        [HttpGet]
        //[Authorize(Roles = "ServerAdmin, Administrator,SupportMember")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            try
            {

                Console.WriteLine("All Users");
                List<ApplicationUser> foundUsers = DatabaseAccesUtil.GetUser();

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
        [HttpPost("Register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] RegisterDto userDto)
        {
            try
            {

                Debug.WriteLine("");
                Debug.WriteLine(userDto.UserName);
                Debug.WriteLine("");
                // Validate the input

                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                UserClient t = new UserClient(userDto);
                IdentityResult result = _userManager.CreateAsync(t.Get(), userDto.PasswordHash).Result;

                if(result.Succeeded)
                {
                    UserRecordArgs args = new UserRecordArgs()
                {
                        Uid = userDto.Email,
                        Email = userDto.Email,
                        Password = UserHandler._PasswordHasher(userDto.PasswordHash,userDto.Email)
                    };
                    try
                    {

                        UserRecord userRecord = FirebaseAuth.DefaultInstance.CreateUserAsync(args).Result;
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex);
                        return Ok(new { Message = "User created successfully, In DB As Fireabse User alredy exits." });
                    }

                    // See the UserRecord reference doc for the contents of userRecord.
                    return Ok(new { Message = "User created successfully" });
                }

                // Return a bad request with detailed error messages if failed
                return BadRequest(result.Errors.Select(e => e.Description));
            }
            catch(Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        // POST: api/<UsersController>
        [HttpPost("Register/Student")]
        [AllowAnonymous]
        public ActionResult RegisterStudent([FromBody] StudentDto userDto)
        {
            try
            {

                Debug.WriteLine("");
                Debug.WriteLine(userDto.UserName);
                Debug.WriteLine("");
                // Validate the input

                //if(!ModelState.IsValid)
                //{
                //    return BadRequest("Faild Due to Modle" + ModelState);
                //}

                UserClient t = new UserClient(userDto);
                IdentityResult result = _userManager.CreateAsync(t.Get(), userDto.PasswordHash).Result;

                if(result.Succeeded)
                {
                    UserRecordArgs args = new UserRecordArgs()
                    {
                        Uid = userDto.Email,
                        Email = userDto.Email,
                        Password = UserHandler._PasswordHasher(userDto.PasswordHash,userDto.Email)
                    };
                    // TODO Uncomment FIRBASE
                    UserRecord userRecord = FirebaseAuth.DefaultInstance.CreateUserAsync(args).Result;

                    // See the UserRecord reference doc for the contents of userRecord.
                    return Ok(new { Message = "User created successfully" });
                }

                // Return a bad request with detailed error messages if failed
                return BadRequest(result.Errors.Select(e => e.Description));
            }
            catch(Exception ex)
            {
                return Problem(ex.ToString());
            }
        }
        // POST: api/<UsersController>
        [HttpPost("Register/Staff")]
        [AllowAnonymous]
        public ActionResult RegisterStaff([FromBody] StaffDto userDto)
        {
            try
            {

                Debug.WriteLine("");
                Debug.WriteLine(userDto.UserName);
                Debug.WriteLine("");
                // Validate the input

                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                UserClient t = new UserClient(userDto);
                IdentityResult result = _userManager.CreateAsync(t.Get(), userDto.PasswordHash).Result;

                if(result.Succeeded)
                {
                    UserRecordArgs args = new UserRecordArgs()
                    {
                        Uid = userDto.Email,
                        Email = userDto.Email,
                        Password = UserHandler._PasswordHasher(userDto.PasswordHash,userDto.Email)
                    };
                    // TODO Uncomment FIRBASE
                    UserRecord userRecord = FirebaseAuth.DefaultInstance.CreateUserAsync(args).Result;

                    // See the UserRecord reference doc for the contents of userRecord.
                    return Ok(new { Message = "User created successfully" });
                }

                // Return a bad request with detailed error messages if failed
                return BadRequest(result.Errors.Select(e => e.Description));
            }
            catch(Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        // POST: api/user/login
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserCredential request)
        {
            try
            {

                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, isPersistent: false, lockoutOnFailure: false);
                Debug.WriteLine(request.Password, "Login");

                if(result.Succeeded)
                {
                    ApplicationUser? user = await _userManager.FindByNameAsync(request.Username);
                    Debug.WriteLine(user.Email);
                    string token = GenerateJwtToken(user);

                    //var uid = "some-uid";

                    //string customToken = FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(request.Username).Result;
                    //Console.WriteLine(customToken);
                    //return customToken;
                    // You may want to retrieve custom properties for claims
                    try
                    {

                        IList<string> roles = await _userManager.GetRolesAsync(user);
                        string role = roles.FirstOrDefault();
                    }
                    catch
                    {
                        Debug.WriteLine("Role Exception");
                    }

                    // Generate JWT Token with claims
                    return Ok(new { token, user });
                }

                return Unauthorized(new { message = $"Invalid username or password {result.ToString()}" });
            }
            catch(Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        // POST: api/user/login
        [Obsolete]
        [HttpPost("login/Student")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginStudent([FromBody] UserCredential request)
        {
            try
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, isPersistent: false, lockoutOnFailure: false);

                if(result.Succeeded)
                {
                    ApplicationUser? user = await _userManager.FindByNameAsync(request.Username);
                    string token = GenerateJwtToken(user);

                    IList<string> roles = await _userManager.GetRolesAsync(user);
                    string role = roles.FirstOrDefault() ?? "Student"; // Default role if none exists

                    // Generate JWT Token with claims
                    UserClient t = new UserClient(user);
                    if(t!= null)
                    {
                        t.Get().GetDetails();
                        StudentDTO student = ((Student)t.Get()).toDto();
                        Console.WriteLine(student.Id);
                        return Ok(new { token, student });
                    }
                    else
                    {
                        return Unauthorized(new { message = "An error occured when proccessing your login" });
                    }
                }

                return Unauthorized(new { message = "Invalid username or password" });
            }
            catch(Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        // Firebase Token
        // Get api/<UsersController>
        [HttpGet("Token/{email}")]
        [Obsolete]
        [AllowAnonymous]
        public ActionResult<string> GetToken(string email)
        {
            try
            {

                Console.WriteLine($"User info for: {email}");
                ApplicationUser foundUser = DatabaseAccesUtil.GetUser(email);
                if(foundUser != null)
                {

                    string customToken = FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(email).Result;
                    Console.WriteLine(customToken);
                    return Ok(customToken);
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

        // JWT Token
        [HttpGet("Student/Info/{email}")]
        [AllowAnonymous]
        public async Task<ActionResult<StudentDTO>> Studentss(string email)
        {
            try
            {

                Console.WriteLine($"Studenr login {email}");
                Console.WriteLine($"Studenr login {Request.Headers}");

                if(email != null)
                {
                    ApplicationUser? user = await _userManager.FindByNameAsync(email);

                    //return customToken;
                    // You may want to retrieve custom properties for claims
                    //var roles = await _userManager.GetRolesAsync(user);
                    //string role = roles.FirstOrDefault() ?? "Student"; // Default role if none exists

                    // Generate JWT Token with claims
                    UserClient t = new UserClient(user);
                    t.Get().GetDetails();
                    return ((Student)t.Get()).toDto();
                }

                return Unauthorized(new { message = "Invalid username or password" });
            }
            catch(Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        // Login Firebase Token
        // POST api/<UsersController>
        [Obsolete]
        [Route("Login/Token")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<string> LoginToken([FromBody] UserCredential cred)
        {

            try
            {
                Console.WriteLine($"User:({cred.Username}) Has Login");
                //var result = UserHandler.VerifyLoginDetails(cred);
                //var result = UserHandler.VerifyLoginDetails(cred);
                Task<Microsoft.AspNetCore.Identity.SignInResult> result = _signInManager.PasswordSignInAsync(cred.Username, cred.Password, isPersistent: false, lockoutOnFailure: false);

                if(result != null)
                {
                    //var uid = "some-uid";

                    string customToken = FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(cred.Username).Result;
                    Console.WriteLine(customToken);
                    return customToken;
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch
            {
                return Problem();
            }
        }

        // GET: api/<UsersController>
        [HttpGet("{id}")]
        //[Authorize(Roles = "ServerAdmin,Administrator,SupportMember")]
        [AllowAnonymous]
        public async Task<ActionResult<ApplicationUser>> GetUser(string id)
        {
            try
            {

                //Azure
                ApplicationUser? t = await _userManager.FindByIdAsync(id.ToString());

                //Debug.WriteLine(t.UserName);

                UserClient user = new UserClient(t);
                //user
                ApplicationUser? actUser = user.Get();
                if(user.Get() == null)
                {
                    return NotFound();
                }
                else
                {
                    Debug.WriteLine(actUser);
                    actUser.GetDetails();
                    return actUser;
                }
            }
            catch
            {
                return NotFound();
            }
        }

        // GET: api/<UsersController>
        [HttpGet("Info")]
        [AllowAnonymous]
        public ActionResult<ApplicationUser> GetUserssdaInfo([FromQuery] string email)
        {
            try
            {

                Console.WriteLine($"User info for: {email}");
                ApplicationUser? t = _userManager.FindByIdAsync(email).Result;
                if(t!= null)
                {

                    UserClient user = new UserClient(t);

                    if(user.Get() == null)
                    {
                        return NotFound();
                    }
                    else
                    {

                        user.Get().GetDetails();
                        return user.Get();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        // GET: api/<UsersController>
        [HttpGet("Info/{email}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApplicationUser>> GetUserssdaInfoPAsync(string email)
        {
            try
            {
                Console.WriteLine($"User info for: {email}");
                ApplicationUser? t = _userManager.FindByNameAsync(email).Result;

                UserClient user = new UserClient(t);

                if(user.Get() == null)
                {
                    return NotFound();
                }
                else
                {

                    user.Get().GetDetails();
                    return user.Get();
                }
            }
            catch(Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        // Get api/<UsersController>
        [HttpGet("Modules/{id}")]
        [AllowAnonymous]
        //public async Task<ActionResult<List<StudentModule>>> GetUserModulesAsync([FromQuery] int? id = null, [FromQuery] string? email = null)
        public async Task<ActionResult<List<StudentModule>>> GetUserModulesAsync(long? id)
        {
            try
            {

                Console.WriteLine($"Get Mudles For: {id}");
                ApplicationUser t;

                t = await _userManager.FindByIdAsync(id.ToString());

                UserClient user = new UserClient(t);

                if(user.Get() == null)
                {
                    return NotFound();
                }
                else
                {
                    if(user.Get().GetType() == typeof(Student))
                    {
                        user.Get().GetDetails();
                        Course? userModules = ((Student)user.Get()).Courses;

                        return userModules.Modules;
                    }
                    else
                    {
                        return Problem("The selected user is not a student");
                    }
                }
            }
            catch(Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        // POST api/<UsersController>
        [Route("AddModule")]
        [HttpPost]
        //[Authorize(Roles = "ServerAdmin,Adminastrator,SupportMember")]
        [AllowAnonymous]
        public ActionResult addModule([FromBody] StaffModule staffMod)
        {
            try
            {

                if(DatabaseAccesUtil.ModuleInsert(staffMod))
                {
                    return Accepted(staffMod.ModuleCode);
                }
                else
                {
                    return Problem();
                }
            }
            catch(Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        // POST api/<UsersController>
        [Route("AddModules/{id}")]
        [HttpPost]
        //[Authorize(Roles = "ServerAdmin,Adminastrator,SupportMember")]
        [AllowAnonymous]
        public ActionResult addModules(long id, [FromBody] List<StaffModule> staffMod)
        {
            try
            {
                Console.WriteLine($"Add mudile TO:{id}");
                Lecturer? result = DatabaseAccesUtil.GetUser(id) as Lecturer;
                if(result != null)
                {
                    if(result.AddMoudles(staffMod))
                    {
                        return Accepted(staffMod.Count);
                    }
                    else
                    {
                        return Problem();
                    }
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

        // POST api/<UsersController>
        [Route("ForgotPassword")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword([FromBody] ForgotPassword fpass)
        {
            try
            {
                ApplicationUser? t = _userManager.FindByNameAsync(fpass.Email).Result;
                string resUser = _userManager.GeneratePasswordResetTokenAsync(t).Result;
                IdentityResult p = _userManager.ResetPasswordAsync(t,resUser,fpass.NewPassword).Result;
                if(p.Succeeded)
                {

                    Debug.WriteLine(fpass.NewPassword);
                    string newPass = UserHandler._PasswordHasher(fpass.NewPassword, fpass.Email);
                    Debug.WriteLine(newPass);
                    Debug.WriteLine(t.PasswordHash);
                    t.PasswordHash = newPass;
                    DatabaseAccesUtil.SetPassword(t.Id, newPass);

                    Debug.WriteLine(t.PasswordHash);
                    return Ok(p);
                }
                else
                {
                    return BadRequest(p.Errors);
                }
            }
            catch(Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        // POST api/<UsersController>
        [Route("fcm/{email}")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult addFcm(string email, [FromBody] UserToken token)
        {
            try
            {

                Console.WriteLine(token.UserId);
                ApplicationUser y = DatabaseAccesUtil.GetUser(email);
                y.fcmToken = token.Token;

                if(y != null)
                {
                    if(y.UpdateFcm(token.Token))
                    {
                        return Accepted();
                    }
                    else
                    {
                        return Problem();
                    }
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



        //
        // POST: api/Account/Logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Ok(new { Message = "Logout successful." });
        }

        // POST: api/Account/ExternalLogin
        [HttpPost("externallogin")]
        [AllowAnonymous]
        public IActionResult ExternalLogin([FromBody] string provider)
        {
            string? redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account");
            Microsoft.AspNetCore.Authentication.AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        // GET: api/Account/ExternalLoginCallback
        [HttpGet("externallogincallback")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string remoteError = null)
        {
            if(remoteError != null)
            {
                return BadRequest(new { Message = $"Error from external provider: {remoteError}" });
            }
            ExternalLoginInfo? info = await _signInManager.GetExternalLoginInfoAsync();
            if(info == null)
            {
                return BadRequest(new { Message = "Error loading external login information." });
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if(result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return Ok(new { Message = "External login successful." });
            }
            else
            {
                return Unauthorized(new { Message = "External login failed." });
            }
        }
    }
}
