using System;
using System.Collections.Generic;

namespace GoFish.Lib.Models
{
    public class GoFishGame
    {
        public GoFishGame()
        {
            Players = new List<Player>();
            GameState = GameState.Waiting;
        }

        public Deck Deck { get; set; }
        public List<Player> Players { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public Player CurrentPlayer
        {
            get => GameState == GameState.Playing ? Players[CurrentPlayerIndex] : null;
            set => CurrentPlayerIndex = Players.IndexOf(value);
        }

        public int NextPlayerIndex
        {
            get
            {
                if (GameState != GameState.Playing) return 0;

                for (int i = 0; i < Players.Count; i++)
                {
                    int index = (CurrentPlayerIndex + i) % Players.Count;
                    if (Players[index].Cards.Count > 0) return index;
                }

                throw new Exception("There is no next player in this game.");
            }
        }

        public Player NextPlayer
            => GameState == GameState.Playing ? Players[NextPlayerIndex] : null;

        public RuleSet RuleSet { get; set; }
        public GameState GameState { get; set; }
    }

    public enum GameState
    {
        Waiting, Playing, Finished
    }
}
