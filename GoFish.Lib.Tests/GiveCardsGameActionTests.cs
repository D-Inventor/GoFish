using System.Collections.Generic;
using System.Linq;

using GoFish.Lib.GameActions;
using GoFish.Lib.Models;
using GoFish.Lib.Tests.Providers;

using NUnit.Framework;

namespace GoFish.Lib.Tests
{
    [TestFixture]
    public class GiveCardsGameActionTests
    {
        [TestCase]
        public void Validate_PlayerIsCurrent_ReturnsFalse()
        {
            GoFishGame game = GameProvider.Game;
            Player player = game.Players[0];
            GiveCardsGameAction ga = new GiveCardsGameAction(new HashSet<Card>(player.Cards.Take(1)), player);

            Game.Lib.Result result = ga.Validate(game);

            Assert.IsFalse(result.Success);
        }

        [TestCase]
        public void Validate_PlayerDoesNotHaveCard_ReturnsFalse()
        {
            GoFishGame game = GameProvider.Game;
            Player player = game.Players[1];
            HashSet<Card> cards = new HashSet<Card>(player.Cards.Skip(1).Append(game.Deck.First()));
            GiveCardsGameAction ga = new GiveCardsGameAction(cards, player);

            Game.Lib.Result result = ga.Validate(game);

            Assert.IsFalse(result.Success);
        }

        [TestCase]
        public void Validate_NotallOfSameCollection_ReturnsFalse()
        {
            GoFishGame game = GameProvider.Game;
            game.RuleSet.GiveCardBehaviour = GiveCardBehaviour.AllOfCollection;
            Player player = game.Players[1];
            GiveCardsGameAction ga = new GiveCardsGameAction(player.Cards, player);

            Game.Lib.Result result = ga.Validate(game);

            Assert.IsFalse(result.Success);
        }

        [TestCase]
        public void Validate_AllOfCollectionNotAllGiven_ReturnsFalse()
        {
            GoFishGame game = GameProvider.Game;
            game.RuleSet.GiveCardBehaviour = GiveCardBehaviour.AllOfCollection;
            Player player = game.Players[1];
            HashSet<Card> cards = new HashSet<Card>(player.Cards.Where(c => c.Collection.Equals("C4")).Take(1));
            GiveCardsGameAction ga = new GiveCardsGameAction(cards, player);

            Game.Lib.Result result = ga.Validate(game);

            Assert.IsFalse(result.Success);
        }

        [TestCase]
        public void Validate_MoreThanOne_ReturnsFalse()
        {
            GoFishGame game = GameProvider.Game;
            game.RuleSet.GiveCardBehaviour = GiveCardBehaviour.Single;
            Player player = game.Players[1];
            HashSet<Card> cards = new HashSet<Card>(player.Cards.Where(c => c.Collection == "C4"));
            GiveCardsGameAction ga = new GiveCardsGameAction(cards, player);

            Game.Lib.Result result = ga.Validate(game);

            Assert.IsFalse(result.Success);
        }

        [TestCase]
        public void Validate_ValidAllOfCollection_ReturnsTrue()
        {

            GoFishGame game = GameProvider.Game;
            game.RuleSet.GiveCardBehaviour = GiveCardBehaviour.AllOfCollection;
            Player player = game.Players[1];
            HashSet<Card> cards = new HashSet<Card>(player.Cards.Where(c => c.Collection == "C4"));
            GiveCardsGameAction ga = new GiveCardsGameAction(cards, player);

            Game.Lib.Result result = ga.Validate(game);

            Assert.IsTrue(result.Success);
        }

        [TestCase]
        public void Validate_ValidSingle_ReturnsTrue()
        {
            GoFishGame game = GameProvider.Game;
            game.RuleSet.GiveCardBehaviour = GiveCardBehaviour.Single;
            Player player = game.Players[1];
            HashSet<Card> cards = new HashSet<Card>(player.Cards.Where(c => c.Collection == "C4").Take(1));
            GiveCardsGameAction ga = new GiveCardsGameAction(cards, player);

            Game.Lib.Result result = ga.Validate(game);

            Assert.IsTrue(result.Success);
        }

        [TestCase]
        public void Apply_Valid_GivesCardToCurrentPlayer()
        {
            GoFishGame game = GameProvider.Game;
            game.RuleSet.GiveCardBehaviour = GiveCardBehaviour.Single;
            Player player = game.Players[1];
            HashSet<Card> cards = new HashSet<Card>(player.Cards.Where(c => c.Collection == "C4").Take(1));
            GiveCardsGameAction ga = new GiveCardsGameAction(cards, player);

            ga.Perform(ref game);

            CollectionAssert.Contains(game.CurrentPlayer.Cards, cards.First());
            CollectionAssert.DoesNotContain(game.Players[1].Cards, cards.First());
        }
    }
}
