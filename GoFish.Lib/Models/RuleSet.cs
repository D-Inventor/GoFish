namespace GoFish.Lib.Models
{
    public class RuleSet
    {
        public TurnBehaviour TurnBehaviour { get; set; }
        public GiveCardBehaviour GiveCardBehaviour { get; set; }
        public StartBehaviour StartBehaviour { get; set; }
        public int CardsOnStart { get; set; }
    }

    public enum TurnBehaviour
    {
        ClockWise, LastAskedPlayer
    }

    public enum GiveCardBehaviour
    {
        AllOfCollection, Single
    }

    public enum StartBehaviour
    {
        FirstPlayer, RandomPlayer
    }
}
