using Game.Lib;

using GoFish.Lib.Models;

namespace GoFish.Lib.Tests.Fakes
{
    internal class ConstantValidityGameAction : IGameAction<GoFishGame>
    {
        private readonly bool validity;

        public ConstantValidityGameAction(bool validity)
        {
            this.validity = validity;
        }

        public bool IsApplied { get; private set; } = false;

        public Result Perform(ref GoFishGame game)
        {
            IsApplied = true;
            return new ResultBuilder().AddFeedback("test", validity).Build();
        }

        public Result Validate(GoFishGame game)
        {

            return new ResultBuilder().AddFeedback("test", validity).Build();
        }
    }
}
