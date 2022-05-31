using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wipro.Business.Models
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(3, ErrorMessage = "O campo {0} precisa ter {1} caracteres", MinimumLength = 3)]
        public string Moeda { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime? DataInicio { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime? DataFim { get; set; }
    }
}
