using BibliotecaAPI.Datos;
using BibliotecaAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController: ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<IEnumerable<Autor>> Get() 
        {
            return await context.Autores.ToListAsync();
            //{ 

            //    Return 
            //    //new Autor{Id=1,Nombre="Vladimir"},
            //    //new Autor{Id=2,Nombre="Ernesto"}
            //};
        }

        [HttpGet("{id:int}")] //api/autores/id
        public async Task<ActionResult<Autor>> Get (int id)
        {
            var autor = await context.Autores
                .Include(x=>x.Libros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (autor is null)
            {
                return NotFound();
            }

            return autor;
        }


        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }



        [HttpPut("{id:int}")]
        [Consumes("application/json")]
        public async Task<ActionResult> Put(int id, [FromBody] Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest("Los ids deben de coincidir");
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        //[HttpPut("{id:int}")] //api/autores/id
        //public async Task<ActionResult>Put(int id, Autor autor)
        //{

        //    if (id != autor.Id)
        //    {
        //        return BadRequest("Los ids deben de coincidir");
        //    }

        //    context.Update(autor);
        //    await context.SaveChangesAsync();
        //    return Ok();

        //}

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            //var registrosBorrados = 
            //await context.Autores.Where(x => x.Id == id).ExecuteDelete();

            int registrosBorrados =
            context.Autores.Where(x => x.Id == id).ExecuteDelete();

            if (registrosBorrados==0)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
