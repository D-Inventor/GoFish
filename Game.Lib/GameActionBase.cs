using System;

namespace Game.Lib
{
    public abstract class GameActionBase<TGameModel> : IGameAction<TGameModel>
    {
        public Result Perform(ref TGameModel game)
        {
            if (!Validate(game).Success) throw new InvalidOperationException($"'{this}' is not a valid action on the given game.");

            return Modify(ref game);
        }

        protected abstract Result Modify(ref TGameModel game);

        public abstract Result Validate(TGameModel game);
    }
}
