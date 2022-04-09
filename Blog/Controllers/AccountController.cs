using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
       [HttpPost("v1/accounts")]
       public async Task<IActionResult> RegisterNewPerfil([FromBody]RegisterViewModel bodyPerfil, [FromServices]BlogDbDataContext context)
       {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErros()));

            var user = new User
            {
                Name = bodyPerfil.Name,
                Email = bodyPerfil.Email,
                Slug = bodyPerfil.Email.Replace("@", "-").Replace(".", "-"),
                Image = "-"
            };

            //geração de senha
            var password = PasswordGenerator.Generate(25);
            user.PasswordHash = PasswordHasher.Hash(password);

            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                var resultUser = new ResultUserViewModel(user);

                return Created($"v1/a/{resultUser.Id}", new ResultViewModel<ResultUserViewModel>(resultUser));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<List<ResultUserViewModel>>("ERR-05X12 Não foi possível incluir a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<ResultUserViewModel>>("ERR-05X013 Falha interna no servidor"));
            }


        }


        [HttpGet("v1/accounts/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
           [FromServices] BlogDbDataContext context,
           [FromRoute] int id)
        {
            try
            {
                var user = await context.Users
                    .FirstOrDefaultAsync(x => x.Id == id);

                var resultUser = new ResultUserViewModel(user);

                if (user == null)
                    return NotFound(new ResultViewModel<List<ResultUserViewModel>>("ERR - 04X01 Usuario não encontrado"));

                return Ok(new ResultViewModel<ResultUserViewModel>(resultUser));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<ResultUserViewModel>>("ERR-04X02 Falha interna no servidor"));
            }
        }


        [HttpPost("v1/logins")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel bodyLogin, [FromServices]TokenService tokenService, [FromServices]BlogDbDataContext context)
        {
            var user = context.Users
                .AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefault(x => x.Email == bodyLogin.Email);

            if (user == null)
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

            if(!PasswordHasher.Verify(user.PasswordHash, bodyLogin.Password))
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

            try
            {
                var token = tokenService.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token, null));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("ERR - 04X03 Falha interna no servidor"));
            }
        }
    }
}
