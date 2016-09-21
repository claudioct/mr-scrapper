using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNetScrapper.Models
{
    public class TournamentRound
    {
        public int Round { get; set; }
        public List<SoccerMatchResult> SoccerMatchResultCollection { get; set; }

    }
}
