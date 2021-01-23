using AttributesAndHandlers.DTOs;
using AttributesAndHandlers.IJWTAuthentication;
using AttributesAndHandlers.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;


namespace AttributesAndHandlers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IJWTAuthenticationManager jWTAuthenticationManager; // jwt Authentication
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager; // Identity Authentication

        public UsersController(IJWTAuthenticationManager jWTAuthenticationManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.jWTAuthenticationManager = jWTAuthenticationManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Authentication Handlers JWT
        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] UserDTO dto)
        {
            var token = jWTAuthenticationManager.Authenticate(dto.Username, dto.Password);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }
        // Authentication Handlers Cookie
        [HttpGet]
        [Route("cookieAuth")]
        public IActionResult CookieAuth()
        {
            var mahyarClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Mgsrkh"),
                new Claim(ClaimTypes.Email,"mahyar.golbaz@gmail.com"),
                new Claim(ClaimTypes.DateOfBirth,"1377/08/14"),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim("Mahyar.Says","Hi There")
            };

            var mahyarIdentity = new ClaimsIdentity(mahyarClaims, "Mahyar.Cookie");

            var userPrincipal = new ClaimsPrincipal(new[] { mahyarIdentity });

            var result = HttpContext.SignInAsync(userPrincipal);

            return Ok(result);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] string username, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email = ""
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Value")]
        [Authorize(Policy = "Claim.Mahyar")] // Policy Base By Cookie // Claim Requirements By Authorization Handler
        public IEnumerable<string> GetPolicy()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("Values")]
        [Authorize(Roles = "Admin")] // Role Base By Cookie
        public IEnumerable<string> GetRole()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            int _id = id;
            return Ok(_id);
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
        [HttpPost] // Login
        public IActionResult Login(UserDTO dto)
        {
            var user = _userManager.FindByNameAsync(dto.Username).Result;
            if (user.PasswordHash == HashGenerator.GenerateHash(dto.Password))
            {
                string token = TokenGenerator.GenerateEncodedToken(user);
                return Ok(token);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
