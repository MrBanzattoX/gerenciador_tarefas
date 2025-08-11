using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GerenciadorTarefas.Models
{
    public enum StatusTarefa
    {
        Pendente,
        EmAndamento,
        Concluida
    }

    public class Tarefa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        public DateTime Data { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;
    }
}