using System;

using Game.Lib;

using GoFish.Lib.Models;
using GoFish.Lib.Providers;

namespace GoFish.Lib.GameActions
{
    public class StartGameAction : GoFishGameActionBase
    {
        private readonly IRandomProvider _randomProvider;

        public StartGameAction(IRandomProvider randomProvider)
        {
            _randomProvider = randomProvider;
        }

        public StartGameAction() : this(RandomProvider.GetInstance())
        { }

        protected override Result Apply(ref GoFishGame game)
        {
            ResultBuilder resultBuilder = new ResultBuilder();
            game.Deck.Shuffle(_randomProvider);

            for (int i = 0; i < game.RuleSet.CardsOnStart; i++)
            {
                foreach (Player player in game.Players)
                {
                    if (game.Deck.TryDraw(out Card card)) player.Cards.Add(card);
                }
            }

            game.GameState = GameState.Playing;
            resultBuilder.AddFeedback($"The game was started!");

            switch (game.RuleSet.StartBehaviour)
            {
                case StartBehaviour.FirstPlayer:
                    game.CurrentPlayerIndex = 0;
                    break;
                case StartBehaviour.RandomPlayer:
                    game.CurrentPlayerIndex = _randomProvider.Next(game.Players.Count);
                    break;
                default:
                    throw new ArgumentException("Unknown option for StartBehaviour", nameof(Models.GoFishGame.RuleSet));
            }
            resultBuilder.AddFeedback($"The turn was passed to {game.CurrentPlayer.Username}");

            return resultBuilder.Build();
        }

        public override Result Validate(GoFishGame game)
        {
            if (game is null)
                throw new ArgumentNullException(nameof(game));

            ResultBuilder resultBuilder = new ResultBuilder();

            if (game.GameState != GameState.Waiting) resultBuilder.AddFeedback($"Cannot start the game because it is not in the 'waiting' state.", false);
            if (game.Players.Count < 2) resultBuilder.AddFeedback($"Cannot start the game because there are less than 2 players.", false);

            return resultBuilder.Build();
        }

        public override string ToString()
        {
            return "Start game";
        }
    }
}