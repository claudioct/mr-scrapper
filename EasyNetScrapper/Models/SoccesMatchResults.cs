using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNetScrapper.Models
{
    public class SoccerMatchResult
    {
        public string AwayScore { get; internal set; }
        public string AwayTeam { get; internal set; }
        public string HomeScore { get; internal set; }
        public string HomeTeam { get; internal set; }
    }
}
