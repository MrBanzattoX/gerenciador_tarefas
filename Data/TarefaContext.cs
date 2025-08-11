using Microsoft.EntityFrameworkCore;
using GerenciadorTarefas.Models;

namespace GerenciadorTarefas.Data
{
    public class TarefaContext : DbContext
    {
        public TarefaContext(DbContextOptions<TarefaContext> options) : base(options)
        {
        }

        public DbSet<Tarefa> Tarefas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarefa>().ToTable("Tarefas");
            modelBuilder.Entity<Tarefa>().HasData(
                new Tarefa { Id = 1, Titulo = "Exemplo", Descricao = "Tarefa de exemplo", Data = DateTime.UtcNow, Status = StatusTarefa.Pendente }
            );
        }
    }
}