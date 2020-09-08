using System;
using System.Linq;

using Game.Lib;

using GoFish.Lib.Models;

namespace GoFish.Lib.GameActions
{
    public class LeaveGameAction : GoFishGameActionBase
    {
        private readonly Player _player;

        public LeaveGameAction(Player player)
        {
            _player = player;
        }

        public override Result Validate(GoFishGame game)
        {
            ResultBuilder resultBuilder = new ResultBuilder();

            if (!game.Players.Contains(_player)) resultBuilder.AddFeedback($"{_player.Username} cannot leave this game because they're not participating.", false);

            return resultBuilder.Build();
        }

        protected override Result Apply(ref GoFishGame game)
        {
            var resultBuilder = new ResultBuilder();
            switch (game.GameState)
            {
                case GameState.Waiting:
                    game.Players.Remove(_player);
                    resultBuilder.AddFeedback($"{_player.Username} left the game.");
                    break;
                case GameState.Playing:
                    Player gameplayer = game.Players.First(p => p.Equals(_player));
                    foreach (Card card in gameplayer.Cards.Concat(gameplayer.FinishedCollections.SelectMany(c => c.Value))) game.Deck.InsertAt(game.Deck.Count - 1, card);
                    game.Deck.Shuffle();
                    game.Players.Remove(gameplayer);
                    resultBuilder.AddFeedback($"{_player.Username} left the game.");
                    break;
                case GameState.Finished:
                    break;
                default:
                    throw new Exception("The given option for GameState is unknown.");
            }

            return resultBuilder.Build();
        }

        public override string ToString()
        {
            return "Leave game";
        }
    }
}