using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/buscar-todos")]
        public IActionResult Get([FromServices] AppDbContext context)
            => Ok(context.Todos.AsNoTracking().ToList());
        

        [HttpGet("/buscar-por-id/{id:int}")]
        public IActionResult GetById([FromRoute]int id, [FromServices] AppDbContext context)
        {
            var todos = context.Todos.AsNoTracking().FirstOrDefault(x=>x.Id==id);
            
            if(todos == null) 
                return NotFound(); 

            return Ok(todos);
        }
        

        [HttpPost("/cadastrar")]
        public IActionResult Post([FromBody] TodoModel todoModel, [FromServices] AppDbContext context)
        {
            context.Todos.Add(todoModel);
            context.SaveChanges();
            return Created("/{todoModel.Id}", todoModel);
        }

        [HttpPut("/atualizar/{id:int}")]
        public IActionResult Put([FromBody] TodoModel todoModel, [FromRoute] int id, [FromServices] AppDbContext context)
        {
            var model = context.Todos.AsNoTracking().FirstOrDefault(x=>x.Id==id);

            if(model == null)
                return NotFound();

             model.Title = todoModel.Title;
             model.Done = todoModel.Done;

             context.Todos.Update(model);
             context.SaveChanges();
             return Ok(model);
        }

        [HttpDelete("/remover/{id:int}")]
        public IActionResult Delete([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var model = context.Todos.AsNoTracking().FirstOrDefault(x=>x.Id==id);

            if(model == null)
            return NotFound();

            context.Todos.Remove(model);
            context.SaveChanges();

            return Ok("Removido com sucesso");
        }
    }
}