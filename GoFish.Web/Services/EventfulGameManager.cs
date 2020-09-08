
using Game.Lib;

using GoFish.Web.Models.Events;

namespace GoFish.Web.Services
{
    public class EventfulGameManager<TGameModel> : IGameManager<TGameModel>
    {
        private readonly IGameManager<TGameModel> _decoratee;
        private readonly IAsyncEventEmitter<GameChange<TGameModel>> _eventEmitter;

        public EventfulGameManager(IGameManager<TGameModel> decoratee, IAsyncEventEmitter<GameChange<TGameModel>> eventEmitter)
        {
            _decoratee = decoratee;
            _eventEmitter = eventEmitter;
        }

        public Result PerformAction(IGameAction<TGameModel> gameAction, ref TGameModel game)
        {
            Result result = _decoratee.PerformAction(gameAction, ref game);
            if (result.Success)
            {
                _eventEmitter.TriggerAsync(_decoratee, new GameChange<TGameModel>(game, result)).Wait();
            }
            return result;
        }
    }
}
