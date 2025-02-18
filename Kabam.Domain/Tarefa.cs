using System.Text.Json.Serialization;

namespace Kabam.Domain
{
    public class Tarefa
    {
        public int Id { get; set; }
        [JsonPropertyName("data")]
        public DateTime Data { get; set; }
        [JsonPropertyName("descricao")]
        public string  Descricao { get; set; }
        [JsonPropertyName("statusTarefa")]
        public StatusTarefaEnum StatusTarefa { get; set; }
        public string UserId { get; set; }
    }
}
