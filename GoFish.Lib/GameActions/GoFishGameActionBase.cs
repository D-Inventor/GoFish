using System.Linq;

using Game.Lib;

using GoFish.Lib.Models;

namespace GoFish.Lib.GameActions
{
    public abstract class GoFishGameActionBase : GameActionBase<GoFishGame>
    {
        protected abstract Result Apply(ref GoFishGame game);

        protected sealed override Result Modify(ref GoFishGame game)
        {
            ResultBuilder resultBuilder = new ResultBuilder();
            resultBuilder.AddResult(Apply(ref game));

            if (game.GameState == GameState.Playing)
            {
                if (IsGameOver(game))
                {
                    game.GameState = GameState.Finished;
                    resultBuilder.AddFeedback("The game is finished!");
                    return resultBuilder.Build();
                }

                HandleEmptyHands(ref game, resultBuilder);
            }

            return resultBuilder.Build();
        }

        private static void HandleEmptyHands(ref GoFishGame game, ResultBuilder resultBuilder)
        {
            if (game.CurrentPlayer.Cards.Count == 0 && game.Deck.TryDraw(out Card card))
            {
                game.CurrentPlayer.Cards.Add(card);
                resultBuilder.AddFeedback($"{game.CurrentPlayer.Username} drew a card from the deck.");
            }
            foreach (Player player in game.Players)
            {
                if (player.Cards.Count == 0 && game.Deck.TryDraw(out card))
                {
                    player.Cards.Add(card);
                    resultBuilder.AddFeedback($"{player.Username} drew a card from the deck");
                }
            }

            if (game.CurrentPlayer.Cards.Count == 0)
            {
                game.CurrentPlayerIndex = game.NextPlayerIndex;
                resultBuilder.AddFeedback($"The turn was passed to {game.CurrentPlayer.Username}");
            }
        }

        private static bool IsGameOver(GoFishGame game)
        {
            return (game.Deck.Count == 0 && !game.Players.Any(p => p.Cards.Count > 0)) || game.Players.Count < 2;
        }
    }
}
