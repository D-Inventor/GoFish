using System;
using System.Collections.Generic;

using GoFish.Lib.GameActions;
using GoFish.Lib.Models;
using GoFish.Lib.Tests.Providers;

using NUnit.Framework;

namespace GoFish.Lib.Tests
{
    [TestFixture]
    public class PassTurnGameActionTests
    {
        [TestCase]
        public void Validate_PlayerIsNotcurrent_ReturnsFalse()
        {
            GoFishGame game = GameProvider.Game;
            Player from = game.Players[1];
            Player to = game.Players[2];
            PassTurnGameAction ga = new PassTurnGameAction(from, to);

            Game.Lib.Result result = ga.Validate(game);

            Assert.IsFalse(result.Success);
        }

        [TestCase]
        public void Validate_ClockWiseNotNextPlayer_ReturnsFalse()
        {
            GoFishGame game = GameProvider.Game;
            game.RuleSet.TurnBehaviour = TurnBehaviour.ClockWise;
            Player from = game.CurrentPlayer;
            Player to = game.Players[2];
            PassTurnGameAction ga = new PassTurnGameAction(from, to);

            Game.Lib.Result result = ga.Validate(game);

            Assert.IsFalse(result.Success);
        }

        [TestCase]
        public void Validate_LastAskedPlayerIsSelf_ReturnsFalse()
        {
            GoFishGame game = GameProvider.Game;
            game.RuleSet.TurnBehaviour = TurnBehaviour.LastAskedPlayer;
            Player from = game.CurrentPlayer;
            Player to = from;
            PassTurnGameAction ga = new PassTurnGameAction(from, to);

            Game.Lib.Result result = ga.Validate(game);

            Assert.IsFalse(result.Success);
        }

        [TestCase]
        public void Validate_LastAskedPlayerHasNoCards_ReturnsFalse()
        {
            GoFishGame game = GameProvider.Game;
            game.RuleSet.TurnBehaviour = TurnBehaviour.LastAskedPlayer;
            Player from = game.CurrentPlayer;
            Player to = game.Players[2];
            to.Cards = new HashSet<Card>();
            PassTurnGameAction ga = new PassTurnGameAction(from, to);

            Game.Lib.Result result = ga.Validate(game);

            Assert.IsFalse(result.Success);
        }

        [TestCase]
        public void Validate_LastAskedPlayerNotInTheGame_ReturnsFalse()
        {
            GoFishGame game = GameProvider.Game;
            game.RuleSet.TurnBehaviour = TurnBehaviour.LastAskedPlayer;
            Player from = game.CurrentPlayer;
            Player to = new Player(Guid.NewGuid(), "Test")
            {
                Cards = CardProvider.HashSetCards
            };
            PassTurnGameAction ga = new PassTurnGameAction(from, to);

            Game.Lib.Result result = ga.Validate(game);

            Assert.IsFalse(result.Success);
        }

        [TestCase]
        public void Validate_ClockwiseValid_ReturnsTrue()
        {
            GoFishGame game = GameProvider.Game;
            game.RuleSet.TurnBehaviour = TurnBehaviour.ClockWise;
            Player from = game.CurrentPlayer;
            Player to = game.NextPlayer;
            PassTurnGameAction ga = new PassTurnGameAction(from, to);

            Game.Lib.Result result = ga.Validate(game);

            Assert.IsTrue(result.Success);
        }

        [TestCase]
        public void Validate_LastAskedPlayerValid_ReturnsTrue()
        {
            GoFishGame game = GameProvider.Game;
            game.RuleSet.TurnBehaviour = TurnBehaviour.LastAskedPlayer;
            Player from = game.CurrentPlayer;
            Player to = game.Players[2];
            PassTurnGameAction ga = new PassTurnGameAction(from, to);

            Game.Lib.Result result = ga.Validate(game);

            Assert.IsTrue(result.Success);
        }


        [TestCase]
        public void Apply_ValidAction_SetsCurrentPlayerToNewPlayer()
        {
            GoFishGame game = GameProvider.Game;
            game.RuleSet.TurnBehaviour = TurnBehaviour.LastAskedPlayer;
            Player from = game.CurrentPlayer;
            Player to = game.Players[2];
            PassTurnGameAction ga = new PassTurnGameAction(from, to);

            ga.Perform(ref game);

            Assert.AreEqual(to, game.CurrentPlayer);
        }
    }
}
