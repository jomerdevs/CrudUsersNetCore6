using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TestBackend.DTOs;
using TestBackend.Models;
using TestBackend.Utilidades;

namespace TestBackend.Controllers
{
    [ApiController]
    [Route("api/v1/user/auth")]
    public class AuthController: ControllerBase
    {
        private readonly DevelopContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration config;

        public AuthController(DevelopContext context, IMapper mapper, IConfiguration config)
        {
            this.context = context;
            this.mapper = mapper;
            this.config = config;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDTO userDto)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var userExists = await context.Users.AnyAsync(x => x.Email == userDto.Email);

                    if (userExists)
                    {
                        return BadRequest(new ResultadoStatusCode
                        {
                            Success = false,
                            Data = null,
                            Message = "Este usuario ya esta registrado"
                        });
                    }

                    if (userDto.Password == userDto.ConfirmPassword)
                    {
                        userDto.Password = PasswordHash.HashSha256(userDto.Password);
                        userDto.Active = 1;
                        userDto.Role = "Usuario"; // Usuario por defecto
                        userDto.CreatedBy = userDto.Name;
                        userDto.CreatedDate = DateTime.Now;

                        var user = mapper.Map<User>(userDto);

                        context.Add(user);
                        await context.SaveChangesAsync();
                        return Ok(user);

                    }

                    return BadRequest("Las contraseñas no coinciden");
                }

                return BadRequest("No pudo registrar, verifique que los campos estén llenados correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                
            }
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDTO user)
        {

            var userFound = await context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

            if (userFound == null)
            {
                return NotFound(new ResultadoStatusCode
                {
                    Success = false,
                    Data = null,
                    Message = "El usuario no existe"
                });
            }

            if (user.Password != null)
            {
                user.Password = PasswordHash.HashSha256(user.Password);

                if (user.Password != userFound.Password)
                {
                    return BadRequest("Contraseña incorrecta");
                }

                var userDB = mapper.Map<User>(user);

                string token = CreateToken(userDB);

                return Ok(token);
            }

            return BadRequest("Debe ingresar una contraseña");
        }

        // Método para generar token
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            var jwt =  new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        
    }
}
