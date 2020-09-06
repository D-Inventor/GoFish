using System;
using System.Collections.Generic;
using System.Linq;

using Game.Lib;

using GoFish.Lib.Models;

namespace GoFish.Lib.GameActions
{
    public class GiveCardsGameAction : GoFishGameActionBase
    {
        private readonly ISet<Card> _cards;
        private readonly Player _player;

        public GiveCardsGameAction(ISet<Card> cards, Player sourceplayer)
        {
            if (cards is null) throw new ArgumentNullException(nameof(cards));
            if (cards.Count == 0) throw new ArgumentException("collection must have at least 1 element.", nameof(cards));
            if (cards.Any(c => c is null)) throw new ArgumentException("collection cannot contain null elements.", nameof(cards));

            _cards = cards;
            _player = sourceplayer ?? throw new ArgumentNullException(nameof(sourceplayer));
        }

        protected override Result Apply(ref Models.GoFishGame game)
        {
            ResultBuilder resultBuilder = new ResultBuilder();

            _player.Cards.ExceptWith(_cards);
            game.CurrentPlayer.Cards.UnionWith(_cards);

            resultBuilder.AddFeedback($"{_player.Username} gave {_cards.Count} card(s) to {game.CurrentPlayer.Username}");

            string collection = _cards.First().Collection;
            ISet<Card> cardsFromCollection = new HashSet<Card>(game.CurrentPlayer.Cards.Where(c => c.Collection.Equals(collection)));
            if (cardsFromCollection.Count >= 4)
            {
                game.CurrentPlayer.FinishedCollections.Add(collection, cardsFromCollection);
                game.CurrentPlayer.Cards.ExceptWith(cardsFromCollection);
                resultBuilder.AddFeedback($"{game.CurrentPlayer.Username} finished the {collection} collection!");
            }

            return resultBuilder.Build();
        }

        public override Result Validate(GoFishGame game)
        {
            if (game is null)
                throw new ArgumentNullException(nameof(game));

            ResultBuilder resultBuilder = new ResultBuilder();

            // You can only give cards when the game is running
            if (game.GameState != GameState.Playing) resultBuilder.AddFeedback("Cards can only be given when the game is in the 'playing' state.", false);

            // You can only give cards if it is not your turn
            if (_player.Equals(game.CurrentPlayer)) resultBuilder.AddFeedback($"{_player.Username} cannot give any cards away because it's their turn to ask.", false);

            // You can only give cards that you have
            if (!_cards.IsSubsetOf(_player.Cards)) resultBuilder.AddFeedback($"{_player.Username} cannot give away these cards, because one or more are not in their possession.", false);

            // You can only give cards according to the card giving rule
            switch (game.RuleSet.GiveCardBehaviour)
            {
                case GiveCardBehaviour.AllOfCollection:
                    string collection = _cards.First().Collection;

                    // Cards must all be from the same collection
                    if (!_cards.All(c => c.Collection.Equals(collection))) resultBuilder.AddFeedback($"Only cards from the same collection can be given away at once.", false);

                    // All cards of the same collection must be given
                    if (_player.Cards.Any(c => c.Collection.Equals(collection) && !_cards.Contains(c))) resultBuilder.AddFeedback($"All cards from the same collection must be given away at once.", false);
                    break;
                case GiveCardBehaviour.Single:
                    if (_cards.Count != 1) resultBuilder.AddFeedback($"Only 1 card can be given away at once.", false);
                    break;
                default:
                    throw new Exception("The given option for GiveCardBehaviour is unknown.");
            }

            return resultBuilder.Build();
        }

        public override string ToString()
        {
            return "Give Cards";
        }
    }
}
