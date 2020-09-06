using System.Collections.Generic;

using GoFish.Lib.Models;

namespace GoFish.Web.Models
{
    public class GameViewModel
    {
        public RuleSet RuleSet { get; set; }
        public int CurrentPlayer { get; set; }
        public int NextPlayer { get; set; }
        public IEnumerable<PlayerViewModel> Players { get; set; }
        public int Deck { get; set; }
        public GameState GameState { get; set; }
        public IEnumerable<CardViewModel> UserCards { get; set; }
    }
}
