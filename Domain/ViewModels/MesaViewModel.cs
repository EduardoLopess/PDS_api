using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels
{
    public class MesaViewModel
    {
        [Required(ErrorMessage = "Número da mesa não informado.")]
        public int? NumeroMesa { get; set; }
        public bool StatusMesa { get; set; }
    }
}