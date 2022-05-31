using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wipro.Console.Models
{
    class Moedas
    {
        public Guid Id { get; set; }

        [JsonProperty("moeda")]
        public string Moeda { get; set; }

        [JsonProperty("dataInicio")]
        public DateTime DataInicio { get; set; }

        [JsonProperty("dataFim")]
        public DateTime DataFim { get; set; }

        [JsonProperty("mensagem")]
        public string Mensagem { get; set; }
    }
}
