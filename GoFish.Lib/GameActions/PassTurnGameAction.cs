using System;
using System.Collections.Generic;
using System.Linq;

using Game.Lib;

using GoFish.Lib.Models;

namespace GoFish.Lib.GameActions
{
    public class PassTurnGameAction : GoFishGameActionBase
    {
        private readonly Player _from;
        private readonly Player _to;

        public PassTurnGameAction(Player from, Player to)
        {
            _from = from ?? throw new ArgumentNullException(nameof(from));
            _to = to ?? throw new ArgumentNullException(nameof(to));
        }

        protected override Result Apply(ref GoFishGame game)
        {
            ResultBuilder resultBuilder = new ResultBuilder();
            if (game.Deck.TryDraw(out Card card))
            {
                game.CurrentPlayer.Cards.Add(card);
                resultBuilder.AddFeedback($"{game.CurrentPlayer.Username} drew a card from the deck.");


                string collection = card.Collection;
                ISet<Card> cardsFromCollection = new HashSet<Card>(game.CurrentPlayer.Cards.Where(c => c.Collection.Equals(collection)));
                if (cardsFromCollection.Count >= 4)
                {
                    game.CurrentPlayer.FinishedCollections.Add(collection, cardsFromCollection);
                    game.CurrentPlayer.Cards.ExceptWith(cardsFromCollection);
                    resultBuilder.AddFeedback($"{game.CurrentPlayer.Username} finished the {collection} collection!");
                }
            }

            game.CurrentPlayer = _to;
            resultBuilder.AddFeedback($"The turn was passed to {game.CurrentPlayer.Username}");

            return resultBuilder.Build();
        }

        public override Result Validate(GoFishGame game)
        {
            if (game is null)
                throw new ArgumentNullException(nameof(game));

            ResultBuilder resultBuilder = new ResultBuilder();

            // cannot pass the turn if the game is not running
            if (game.GameState != GameState.Playing) resultBuilder.AddFeedback("Turns can only be passed of the game is in the 'playing' state.", false);

            // cannot pass the turn if you are not the current player
            if (!_from.Equals(game.CurrentPlayer)) resultBuilder.AddFeedback($"{_from.Username} cannot pass the turn because it's not their turn.", false);

            // can only pass the turn according to the rules
            switch (game.RuleSet.TurnBehaviour)
            {
                case TurnBehaviour.ClockWise:

                    // can not pass the turn to a player that is not the next
                    if (!_to.Equals(game.NextPlayer)) resultBuilder.AddFeedback($"The turn cannot be passed to {_to.Username}, because they're not the next clockwise player.", false);
                    break;
                case TurnBehaviour.LastAskedPlayer:
                    // cannot pass the turn to somebody who is not in the game
                    if (!game.Players.Contains(_to)) resultBuilder.AddFeedback("The turn can only be passed to a player that actually participates in this game.", false);

                    // cannot pass the turn to yourself
                    if (_to.Equals(_from)) resultBuilder.AddFeedback("The turn must be passed to a different player.", false);

                    // cannot pass the turn to somebody who has no cards
                    if (_to.Cards.Count == 0) resultBuilder.AddFeedback("The turn cannot be passed to a player who has no cards.", false);
                    break;
                default:
                    throw new Exception("The given option for TurnBehaviour is unknown.");
            }

            return resultBuilder.Build();
        }

        public override string ToString()
        {
            return "Pass turn";
        }
    }
}
