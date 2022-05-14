using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet("v1/posts")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDbDataContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var count = await context.Posts.CountAsync();
                var posts = await context.Posts
                    .AsNoTracking()
                    .Include(p => p.Category)
                    .Include(p => p.Author)
                    .Select(p => new ListPostsViewModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Slug = p.Slug,
                        LastUpdateDate = p.LastUpdateDate,
                        Category = p.Category.Name,
                        Author = $"{p.Author.Name} ({p.Author.Email})"
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(p => p.LastUpdateDate)
                    .ToListAsync();
                return Ok(new ResultViewModel<dynamic>(new
                {
                    total = count,
                    page,
                    pageSize,
                    posts
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Post>>("ERR-11X01 Falha interna no servidor"));
            }
        }


        [HttpGet("v1/posts/{id:int}")]
        public async Task<IActionResult> DetailsAsync(
            [FromServices] BlogDbDataContext context,
            [FromRoute] int id)
        {
            try
            {
                var post = await context.Posts
                    .Include(p => p.Category)
                    .Include(p => p.Author)
                    .ThenInclude(a => a.Roles)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (post == null)
                    return NotFound(new ResultViewModel<Post>("Conteudo não Encontrado"));

                return Ok(new ResultViewModel<Post>(post));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Post>>("ERR-11X02 Falha interna no servidor"));
            }
        }

        [HttpGet("v1/posts/category/{category}")]
        public async Task<IActionResult> GetByCategoryAsync(
            [FromServices] BlogDbDataContext context,
            [FromRoute] string category,
            [FromQuery]int page = 0,
            [FromQuery]int pageSize = 20)
        {
            var count = await context.Posts.AsNoTracking().CountAsync();
            try
            {

                var posts = await context.Posts
                    .AsNoTracking()
                    .Include(p => p.Category)
                    .Include(p => p.Author)
                    .Where(p => p.Category.Slug == category)
                    .Select(p => new ListPostsViewModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Slug = p.Slug,
                        LastUpdateDate = p.LastUpdateDate,
                        Category = p.Category.Name,
                        Author = $"{p.Author.Name} ({p.Author.Email})"
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(p => p.LastUpdateDate)
                    .ToListAsync();

                if (posts.Count == 0)
                    return NotFound(new ResultViewModel<Post>("Conteudo não Encontrado"));

                return Ok(new ResultViewModel<dynamic>(new
                {
                    total = count,
                    page,
                    pageSize,
                    posts
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Post>>("ERR-11X03 Falha interna no servidor"));
            }
        }

    }
}
