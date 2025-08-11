using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciadorTarefas.Data;
using GerenciadorTarefas.Models;

namespace GerenciadorTarefas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaContext _context;

        public TarefaController(TarefaContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Tarefa>> GetById(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) return NotFound();
            return Ok(tarefa);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, Tarefa tarefa)
        {
            if (id != tarefa.Id) return BadRequest("Id do caminho diferente do Id no body.");

            var exists = await _context.Tarefas.AnyAsync(t => t.Id == id);
            if (!exists) return NotFound();

            _context.Entry(tarefa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Tarefas.AnyAsync(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) return NotFound();

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("ObterTodos")]
        public async Task<ActionResult<IEnumerable<Tarefa>>> ObterTodos()
        {
            var lista = await _context.Tarefas.OrderBy(t => t.Data).ToListAsync();
            return Ok(lista);
        }

        [HttpGet("ObterPorTitulo")]
        public async Task<ActionResult<IEnumerable<Tarefa>>> ObterPorTitulo([FromQuery] string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo)) return BadRequest("O parâmetro 'titulo' é obrigatório.");

            var lista = await _context.Tarefas
                .Where(t => EF.Functions.Like(t.Titulo, $"%{titulo}%"))
                .OrderBy(t => t.Data)
                .ToListAsync();
            return Ok(lista);
        }

        [HttpGet("ObterPorData")]
        public async Task<ActionResult<IEnumerable<Tarefa>>> ObterPorData([FromQuery] DateTime data)
        {
            var lista = await _context.Tarefas
                .Where(t => t.Data.Date == data.Date)
                .OrderBy(t => t.Data)
                .ToListAsync();
            return Ok(lista);
        }

        [HttpGet("ObterPorStatus")]
        public async Task<ActionResult<IEnumerable<Tarefa>>> ObterPorStatus([FromQuery] StatusTarefa status)
        {
            var lista = await _context.Tarefas
                .Where(t => t.Status == status)
                .OrderBy(t => t.Data)
                .ToListAsync();
            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult<Tarefa>> Post(Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = tarefa.Id }, tarefa);
        }
    }
}