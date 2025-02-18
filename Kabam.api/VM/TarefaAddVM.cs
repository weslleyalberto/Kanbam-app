
using Kabam.Domain;
using System.ComponentModel.DataAnnotations;

namespace Kabam.api.VM
{
    public struct TarefaAddVM
    {
        [Required]
        public string Data { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public StatusTarefaEnum StatusTarefa { get; set; }
    }
}
