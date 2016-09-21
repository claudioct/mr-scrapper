using EasyNetScrapper.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNetScrapper
{
    public class MrBrain : MrRobot
    {
        public MrBrain()
        {
            this.MrRobotWebClient = new MrRobotWebClient();
        }

        public List<TournamentRound> RetrieveData()
        {
            List<TournamentRound> tournamentRoundCollection = new List<TournamentRound>();
            NameValueCollection parametros = new NameValueCollection();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            this.MrRobotWebClient._allowAutoRedirect = false;

            for (int i = 0; i <= 38; i++)
            {
                var ret = this.HttpGet($@"http://globoesporte.globo.com/servico/esportes_campeonato/responsivo/widget-uuid/bc112c6b-2a2e-4620-8f01-1f31cfa6af5c/fases/fase-unica-seriea-2016/rodada/{i}/jogos.html");

                var results = ret.DocumentNode.Descendants("li").Where(d => (d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("lista-de-jogos-item")));

                List<SoccerMatchResult> soccerMatchResultCollection = new List<SoccerMatchResult>();

                foreach (var item in results)
                {
                    SoccerMatchResult soccerMatchResult = new SoccerMatchResult();

                    //Carregando o Html de cada artigo.
                    doc.LoadHtml(item.InnerHtml);

                    //Estou utilizando o HtmlAgilityPack.HtmlEntity.DeEntitize para fazer o HtmlDecode dos textos capturados de cada artigo.
                    // Utilizo também o UTF8 para limpar o restante dos Encodes que estiverem na página.
                    var matchData = doc.DocumentNode.Descendants("div").Where(d => (d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("placar-jogo-equipes"))).FirstOrDefault();

                    var homeTeamData = matchData.Descendants("span").Where(d => (d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("placar-jogo-equipes-mandante")));
                    var awayTeamData = matchData.Descendants("span").Where(d => (d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("placar-jogo-equipes-visitante")));
                    var scoreData = matchData.Descendants("span").Where(d => (d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("placar-jogo-equipes-placar")));

                    soccerMatchResult.HomeTeam = HtmlAgilityPack.HtmlEntity.DeEntitize(ConvertUTF(homeTeamData.FirstOrDefault().Descendants("span").Where(d => (d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("placar-jogo-equipes-nome"))).FirstOrDefault().InnerText));
                    soccerMatchResult.AwayTeam = HtmlAgilityPack.HtmlEntity.DeEntitize(ConvertUTF(awayTeamData.FirstOrDefault().Descendants("span").Where(d => (d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("placar-jogo-equipes-nome"))).FirstOrDefault().InnerText));
                    soccerMatchResult.HomeScore = HtmlAgilityPack.HtmlEntity.DeEntitize(ConvertUTF(scoreData.FirstOrDefault().Descendants("span").Where(d => (d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("placar-jogo-equipes-placar-mandante"))).FirstOrDefault().InnerText));
                    soccerMatchResult.AwayScore = HtmlAgilityPack.HtmlEntity.DeEntitize(ConvertUTF(scoreData.FirstOrDefault().Descendants("span").Where(d => (d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("placar-jogo-equipes-placar-visitante"))).FirstOrDefault().InnerText));

                    if (string.IsNullOrWhiteSpace(soccerMatchResult.HomeScore))
                    {
                        continue;
                    }

                    soccerMatchResultCollection.Add(soccerMatchResult);
                }
                TournamentRound tournamentRound = new TournamentRound();
                tournamentRound.Round = i;
                tournamentRound.SoccerMatchResultCollection = soccerMatchResultCollection;

                tournamentRoundCollection.Add(tournamentRound);
            }
            return tournamentRoundCollection;
        }            
    

        private string ConvertUTF(string texto)
        {
            // Convertendo o texto para o Enconding default e Array de bytes.
            byte[] data = Encoding.Default.GetBytes(texto);

            //Convertendo o texto limpo para UTF8.
            string ret = Encoding.UTF8.GetString(data);

            return ret;
        }

    }
}
