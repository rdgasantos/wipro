using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Wipro.Console.Models;

namespace Wipro.Console
{
    class Program
    {
        static string pathBase = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("Wipro.Console")) + @"Wipro.Console\Docs";
        static bool flag = true;
        static void Main(string[] args)
        {
            System.Console.WriteLine("###### TESTE WIPRO ######");
            System.Console.WriteLine("------- DESAFIO 2 -------");

            var task = new Thread(Execute);
            task.Start();
        }

        static async void Execute()
        {
            var listaDadosCotacao = ListDadosCotacaoCsv();

            int seq = 0;
            while (flag)
            {
                seq++;
                var time = DateTime.Now;
                var item = await GetLastItem();
                var listaItens = ListDadosMoedaCsv(item);
                var listaCotacao = ListDadosCotacaoCsv();
                var listaMoedaCotacao = ListMoedaCotacaoJson();

                var result = (from p in listaItens
                                 join d in listaMoedaCotacao on p.ID_MOEDA equals d.ID_MOEDA
                                 join m in listaCotacao on d.cod_cotacao equals m.cod_cotacao
                                 where m.dat_cotacao == p.DATA_REF
                                 select new ResultadoMoedaCotacao
                                 {
                                     ID_MOEDA = p.ID_MOEDA,
                                     DATA_REF = m.dat_cotacao,
                                     VL_COTACAO = m.vlr_cotacao
                                 }).ToList();

                if (result.Count > 0)
                {
                    ExportResultadoCsv(result);
                }

                var Time = DateTime.Now - time;
                string restTime = Time.ToString();
                System.Console.Write($"\nA Iteração: {seq} executou no tempo de: {restTime} a moeda: {item.Moeda}, {item.Mensagem}\n");
                System.Console.WriteLine("Próxima execução em 2 minutos.");
                Thread.Sleep(TimeSpan.FromSeconds(120));
            }
        }

        static async Task<Moedas> GetLastItem()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44392/api/Itens");

                    var result = client.GetAsync("Itens").Result;

                    if (result.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<Moedas>(await result.Content.ReadAsStringAsync());
                    }

                    if(result.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return new Moedas() { Mensagem = "Não houve dados para essa requisição." };

                    return new Moedas() {  Mensagem = "Retorno sem sucesso." };
                }
            }
            catch (Exception ex)
            {
                return new Moedas()
                {
                    
                    Mensagem = ex.Message.Contains("No connection could be made because the target machine actively refused it")
                    ? "Sem comunicacao com API, verificar servico WEBAPI..." : ex.Message
                };
            }

        }

        static List<DadosMoeda> ListDadosMoedaCsv(Moedas moeda)
        {

            var listDadosMoeda = new List<DadosMoeda>();
            var lista = File.ReadAllLines(@$"{pathBase}\DadosMoeda.csv");
            foreach (var l in lista)
            {

                if (l != "ID_MOEDA;DATA_REF")
                {
                    string[] values = l.Split(';');
                    listDadosMoeda.Add(new DadosMoeda()
                    {
                        ID_MOEDA = values[0].ToString(),
                        DATA_REF = Convert.ToDateTime(values[1])
                    });
                }
            }

            return listDadosMoeda.Where(
                    x => x.ID_MOEDA == moeda.Moeda
                    && x.DATA_REF >= moeda.DataInicio
                    && x.DATA_REF <= moeda.DataFim).ToList();
        }

        static List<DadosCotacao> ListDadosCotacaoCsv()
        {
            var listDadosCotacao = new List<DadosCotacao>();
            var lista = File.ReadAllLines(@$"{pathBase}\DadosCotacao.csv");
            foreach (var l in lista)
            {

                if (l != "vlr_cotacao;cod_cotacao;dat_cotacao")
                {
                    string[] values = l.Split(';');
                    listDadosCotacao.Add(new DadosCotacao()
                    {
                        vlr_cotacao = decimal.Parse(values[0].ToString()),
                        cod_cotacao = int.Parse(values[1].ToString()),
                        dat_cotacao = Convert.ToDateTime(values[2])
                    });
                }
            }

            return listDadosCotacao.ToList();
        }

        private static List<MoedaCotacao> ListMoedaCotacaoJson()
        {
            var listMoedaCotacao = new List<MoedaCotacao>();
            using (StreamReader r = new StreamReader(pathBase + @"\moedacotacao.json"))
            {

                string data = r.ReadToEnd();
                listMoedaCotacao = JsonConvert.DeserializeObject<List<MoedaCotacao>>(data);
            }

            return listMoedaCotacao;
        }

        private static void ExportResultadoCsv(List<ResultadoMoedaCotacao> resultado)
        {
            var archiveName = "Resultado_" + DateTime.Now.ToString("yyyymmdd_HHmmss") + ".csv";

            using (var file = File.CreateText(pathBase + @"\" + archiveName))
            {
                file.WriteLine("ID_MOEDA;DATA_REF;VL_COTACAO");
                foreach (var r in resultado)
                {
                    file.WriteLine(r.ID_MOEDA + ";" + r.DATA_REF + ";" + r.VL_COTACAO);
                }
            }
        }
    }
}
