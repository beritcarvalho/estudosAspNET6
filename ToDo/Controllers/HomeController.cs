using Microsoft.AspNetCore.Mvc;
using ToDo.Data;
using ToDo.Models;

namespace ToDo.Controllers
{
    [ApiController]
    [Route("home")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("/MetGet")]
        public IActionResult Get([FromServices] AppDbContext context)
            => Ok(context.Todos.ToList());


        [HttpGet]
        [Route("/MetGet/{id:int}")]
        public IActionResult GetById([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var todoRecuperado = context.Todos.FirstOrDefault(x => x.Id == id);

            if (todoRecuperado == null)
                return NotFound();

            return Ok(todoRecuperado);
        }

        [HttpPost]
        [Route("/PostQualquer")]
        public IActionResult PostQualquer([FromServices] AppDbContext context)
        {
            var teste = new TodoModel();
            teste.Titulo = "Inserção manual";
            teste.Concluido = true;
            teste.DataCriacao = DateTime.Now;
            context.Todos.Add(teste);
            context.SaveChanges();
            return Created($"/{teste.Id}", teste);
        }

        [HttpPost("/MetPost")]
        public IActionResult Post([FromBody] TodoModel corpoDoEvento, [FromServices] AppDbContext context)
        {
            if (corpoDoEvento.DataCriacao == new DateTime())
                corpoDoEvento.DataCriacao = DateTime.Now;

            context.Todos.Add(corpoDoEvento);
            context.SaveChanges();
            return Created($"{corpoDoEvento.Id}", corpoDoEvento);
        }

        [HttpPut]
        [Route("/MetPut/{id:int}")]
        public IActionResult Put([FromRoute] int id, [FromBody] TodoModel corpoEvento, [FromServices] AppDbContext context)
        {
            var dadoRecuperado = context.Todos.FirstOrDefault(x => x.Id == id);

            if (dadoRecuperado == null)
                return NotFound();

            dadoRecuperado.Titulo = corpoEvento.Titulo;
            dadoRecuperado.Concluido = corpoEvento.Concluido;

            context.Todos.Update(dadoRecuperado);
            context.SaveChanges();

            return Ok(dadoRecuperado);
        }

        [HttpDelete]
        [Route("/MetDel/{id:int}")]
        public IActionResult Delete([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var dadoRecupeado = context.Todos.FirstOrDefault(x => x.Id == id);
            if (dadoRecupeado == null)
                return NotFound("Dado não encontrado");

            context.Todos.Remove(dadoRecupeado);
            context.SaveChanges();

            return Ok(dadoRecupeado);
        }
    }
}
