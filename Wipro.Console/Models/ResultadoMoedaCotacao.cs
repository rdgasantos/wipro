using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wipro.Console.Models
{
    class ResultadoMoedaCotacao
    {
        public string ID_MOEDA { get; set; }
        public DateTime DATA_REF { get; set; }
        public decimal VL_COTACAO { get; set; }
    }
}
