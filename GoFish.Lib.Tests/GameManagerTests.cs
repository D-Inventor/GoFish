
using Game.Lib;

using GoFish.Lib.Models;
using GoFish.Lib.Tests.Fakes;
using GoFish.Lib.Tests.Providers;

using NUnit.Framework;

namespace GoFish.Lib.Tests
{
    [TestFixture]
    public class GameManagerTests
    {
        [TestCase]
        public void GameManager_ValidTurn_PerformsTurn()
        {
            ConstantValidityGameAction action = new ConstantValidityGameAction(true);
            GameManager<GoFishGame> gm = new GameManager<GoFishGame>();
            GoFishGame game = GameProvider.Game;

            gm.PerformAction(action, ref game);

            Assert.IsTrue(action.IsApplied);
        }

        [TestCase]
        public void GameManager_InvalidTurn_ThrowsInvalidOperationException()
        {
            ConstantValidityGameAction action = new ConstantValidityGameAction(false);
            GameManager<GoFishGame> gm = new GameManager<GoFishGame>();
            GoFishGame game = GameProvider.Game;

            Result result = gm.PerformAction(action, ref game);

            Assert.IsFalse(result.Success);
        }
    }
}
