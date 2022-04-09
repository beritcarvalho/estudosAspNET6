using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.Extensions;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDbDataContext context)
        {
            try
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("ERR-05X01 Falha interna no servidor"));
            }
        }


        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] BlogDbDataContext context, 
            [FromRoute]int id)
        {
            try
            {
                var category = await context.Categories
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<List<Category>>("ERR - 05X02 Categoria não encontrada"));

                return Ok(new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("ERR-05X03 Falha interna no servidor"));
            }
        }


        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
        [FromServices] BlogDbDataContext context,
        [FromBody] EditorCategoryViewModel corpoEvento)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErros()));

            try
            {
                var category = new Category
                {
                    Id = 0,
                    Name = corpoEvento.Name,
                    Slug = corpoEvento.Slug.ToLower(),
                    Posts = null
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("ERR-05X04 Não foi possível incluir a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("ERR-05X011 Falha interna no servidor"));
            }

        }


        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync([FromServices]BlogDbDataContext context, [FromBody]EditorCategoryViewModel corpoEvento, [FromRoute]int id)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
 
                if (category == null)
                    return NotFound(new ResultViewModel<List<Category>>("ERR - 05X10 Categoria não encontrada"));

                category.Name = corpoEvento.Name;
                category.Slug = corpoEvento.Slug;

                context.Update(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("ERR-05X05 Não foi possível atualizar a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("ERR-05X06 Falha interna no servidor"));
            }
        }


        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromServices]BlogDbDataContext context, [FromBody]Category corpoEvento, [FromRoute]int id)
        {
            try
            {
                var categoriaRemover = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (categoriaRemover == null)
                    return NotFound(new ResultViewModel<List<Category>>("ERR - 05X09 Não foi possível incluir a categoria"));

                context.Remove(categoriaRemover);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(categoriaRemover));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("ERR-05X07 Não foi remover a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("ERR-05X08 Falha interna no servidor"));
            }  
        }
    }
}
