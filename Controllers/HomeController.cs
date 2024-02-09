using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/home")]
        public IEnumerable<TodoModel> Get([FromServices] AppDbContext context)
        {
            return context.Todos.AsNoTracking().ToList();
        }

        [HttpGet("/home/{id:int}")]
        public TodoModel GetById([FromRoute]int id, [FromServices] AppDbContext context)
        {
            return context.Todos.AsNoTracking().FirstOrDefault(x=>x.Id==id);
        }

        [HttpPost("/cadastrar")]
        public TodoModel Post([FromBody] TodoModel todoModel, [FromServices] AppDbContext context)
        {
            context.Todos.Add(todoModel);
            context.SaveChanges();
            return todoModel;
        }

        [HttpPut("/atualizar/{id:int}")]
        public TodoModel Put([FromBody] TodoModel todoModel, [FromRoute] int id, [FromServices] AppDbContext context)
        {
            var model = context.Todos.AsNoTracking().FirstOrDefault(x=>x.Id==id);

            if(model == null)
                return model;

             model.Title = todoModel.Title;
             model.Done = todoModel.Done;

             context.Todos.Update(model);
             context.SaveChanges();
             return model;
        }

        [HttpDelete("/remover/{id:int}")]
        public string Delete([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var model = context.Todos.AsNoTracking().FirstOrDefault(x=>x.Id==id);

            if(model == null)
            return "Código inválido";

            context.Todos.Remove(model);
            context.SaveChanges();

            return "Removido com sucesso";
        }
    }
}