using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestBackend.DTOs;
using TestBackend.Models;
using TestBackend.Utilidades;

namespace TestBackend.Controllers
{    
    [ApiController]
    [Route("api/v1/user")]
    public class UserController: ControllerBase
    {
        private readonly DevelopContext context;
        private readonly IMapper mapper;

        public UserController(DevelopContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers(string search)
        {
            try
            {
                if (!String.IsNullOrEmpty(search))
                {
                    return await context.Users.Where(s => s.Name.ToUpper().Contains(search.ToUpper())
                                || s.Lastname.ToUpper().Contains(search.ToUpper()) || s.Email.ToUpper().Contains(search.ToUpper())).ToListAsync();
                }

                return await context.Users.ToListAsync();
                    
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                
            }
        }

        // Get by id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                return user;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateUserDTO userDto)
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

                    userDto.Password = PasswordHash.HashSha256(userDto.Password);
                    userDto.Active = 1;                    

                    var user = mapper.Map<User>(userDto);

                    context.Add(user);
                    await context.SaveChangesAsync();
                    return Ok("Usuario creado exitosamente!!");
                }                

                return BadRequest("No se puede crear usuario");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateUser(User user, int id)
        {            
            try
            {              
                
                var entidad = context.Users.FirstOrDefault(x => x.Id == id);

                if (entidad == null)
                {
                    return NotFound( new ResultadoStatusCode
                    {
                        Success = false,
                        Data = null,
                        Message = "El usuario no existe"
                    });
                }

                // Si se actualiza la contraseña
                if (user.Password != entidad.Password)
                {
                    user.Password = PasswordHash.HashSha256(user.Password);                    
                }
                
                entidad.Username = user.Username;
                entidad.Password = user.Password;
                entidad.Name = user.Name;
                entidad.Lastname = user.Lastname;
                entidad.Email = user.Email;
                entidad.Active = user.Active;
                entidad.Role = user.Role;

                context.Update(entidad);
                await context.SaveChangesAsync();
                return Ok("Usuario actualizado correctamente");
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PatchUser(int id, JsonPatchDocument<PatchUserDTO> patchDocument)
        {
            try
            {
                // Si hay un error con el formato enviado
                if (patchDocument == null)
                {
                    return BadRequest();
                }

                var userDB = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (userDB == null)
                {
                    return NotFound(new ResultadoStatusCode
                    {
                        Success = false,
                        Data = null,
                        Message = "El usuario no existe"
                    });
                }                

                var user = mapper.Map<PatchUserDTO>(userDB);

                patchDocument.ApplyTo(user, ModelState);

                var isValid = TryValidateModel(user);

                if (!isValid)
                {
                    return BadRequest(ModelState);
                }

                mapper.Map(user, userDB);

                await context.SaveChangesAsync();
                return Ok("Datos actualizados correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var existe = await context.Users.AnyAsync(x => x.Id == id);

                if (!existe)
                {
                    return NotFound();
                }

                context.Remove(new User() { Id = id });
                await context.SaveChangesAsync();
                return Ok("Usuario borrado exitosamente!");
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);                
            }

        }
    }
}
