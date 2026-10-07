using GarmentsAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace GarmentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserRepository userRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public ActionResult GetUsers()
        {
            try
            {
                var users = _userRepository.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new { message = ex.Message }
                );
            }
        }
        [AllowAnonymous]
        [HttpGet("setup-status")]
        public ActionResult GetSetupStatus()
        {
            try
            {
                var users = _userRepository.GetUsers();

                bool setupRequired =
                    users == null || users.Count == 0;

                return Ok(new
                {
                    setupRequired = setupRequired,
                    userCount = users?.Count ?? 0
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }
        // ====================================================
        // LOGIN
        // ====================================================

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login(
            [FromBody] LoginRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new
                {
                    message =
                        "Username and password are required."
                });
            }


            var user =
                _userRepository.GetUserByUsername(
                    request.Username.Trim()
                );


            if (user == null)
            {
                return Unauthorized(new
                {
                    message =
                        "Invalid username or password."
                });
            }


            if (!user.IsActive)
            {
                return Unauthorized(new
                {
                    message =
                        "User account is inactive."
                });
            }


            bool validPassword =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    user.PasswordHash
                );


            if (!validPassword)
            {
                return Unauthorized(new
                {
                    message =
                        "Invalid username or password."
                });
            }


            string token =
                GenerateToken(user);


            return Ok(new
            {
                token = token,

                user = new
                {
                    userID = user.UserID,
                    username = user.Username,
                    role = user.Role
                }
            });
        }


        // ====================================================
        // GENERATE JWT
        // ====================================================

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserID.ToString()
                ),

                new Claim(
                    ClaimTypes.Name,
                    user.Username
                ),

                new Claim(
                    ClaimTypes.Role,
                    user.Role
                )
            };


            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _configuration["Jwt:Key"]
                    )
                );


            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256
                );


            var expiry =
                Convert.ToDouble(
                    _configuration["Jwt:ExpiryMinutes"]
                );


            var token =
                new JwtSecurityToken(
                    issuer:
                        _configuration["Jwt:Issuer"],

                    audience:
                        _configuration["Jwt:Audience"],

                    claims: claims,

                    expires:
                        DateTime.UtcNow.AddMinutes(expiry),

                    signingCredentials:
                        credentials
                );


            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }


        // ====================================================
        // CREATE USER
        // ADMIN ONLY
        // ====================================================

        [Authorize(Roles = "Admin")]
        [HttpPost("create-user")]
        public ActionResult CreateUser(
            [FromBody] CreateUserRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.Role))
            {
                return BadRequest(new
                {
                    message =
                        "Username, password and role are required."
                });
            }


            string[] validRoles =
            {
                "Admin",
                "Manager",
                "Staff",
                "Viewer"
            };


            if (!validRoles.Contains(request.Role))
            {
                return BadRequest(new
                {
                    message =
                        "Invalid role."
                });
            }


            var existingUser =
                _userRepository.GetUserByUsername(
                    request.Username.Trim()
                );


            if (existingUser != null)
            {
                return Conflict(new
                {
                    message =
                        "Username already exists."
                });
            }


            string passwordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.Password
                );


            var user = new User
            {
                Username =
                    request.Username.Trim(),

                PasswordHash =
                    passwordHash,

                Role =
                    request.Role,

                IsActive =
                    true
            };


            int userID =
                _userRepository.AddUser(user);


            return Ok(new
            {
                message =
                    "User created successfully.",

                userID = userID
            });
        }
        [Authorize]
        [HttpGet("debug-auth")]
        public IActionResult DebugAuth()
        {
            return Ok(new
            {
                IsAuthenticated = User.Identity?.IsAuthenticated,
                Name = User.Identity?.Name,
                Role = User.FindFirst(ClaimTypes.Role)?.Value,

                Claims = User.Claims.Select(c => new
                {
                    Type = c.Type,
                    Value = c.Value
                })
            });
        }
        // ====================================================
        // FIRST ADMIN
        // REMOVE THIS AFTER INITIAL SETUP
        // ====================================================

        [AllowAnonymous]
        [HttpPost("create-first-admin")]
        public ActionResult CreateFirstAdmin(
            [FromBody] CreateUserRequest request)
        {
            var users =
                _userRepository.GetUsers();


            if (users.Any())
            {
                return BadRequest(new
                {
                    message =
                        "Users already exist. Use an Admin account."
                });
            }


            if (request == null ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new
                {
                    message =
                        "Username and password are required."
                });
            }


            string passwordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.Password
                );


            var user = new User
            {
                Username =
                    request.Username.Trim(),

                PasswordHash =
                    passwordHash,

                Role =
                    "Admin",

                IsActive =
                    true
            };


            int userID =
                _userRepository.AddUser(user);


            return Ok(new
            {
                message =
                    "First Admin created successfully.",

                userID = userID
            });
        }
    }
}