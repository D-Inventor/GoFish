using System;

namespace Game.Lib
{
    public class GameManager<TGameModel> : IGameManager<TGameModel>
    {
        public Result PerformAction(IGameAction<TGameModel> gameAction, ref TGameModel game)
        {
            Result result = gameAction.Validate(game);
            if (!result.Success) return result;

            try
            {
                return gameAction.Perform(ref game);
            }
            catch (Exception e)
            {
                return Result.Create(false, new[] { $"{gameAction} was valid, but threw an exception: {e.Message}" });
            }
        }
    }
}
