
using Game.Lib;

using GoFish.Web.Models.Events;

namespace GoFish.Web.Services
{
    public class EventfulGameManager<TGameModel> : IGameManager<TGameModel>
    {
        private readonly IGameManager<TGameModel> _decoratee;
        private readonly IEventEmitter<GameChange<TGameModel>> _eventEmitter;

        public EventfulGameManager(IGameManager<TGameModel> decoratee, IEventEmitter<GameChange<TGameModel>> eventEmitter)
        {
            _decoratee = decoratee;
            _eventEmitter = eventEmitter;
        }

        public Result PerformAction(IGameAction<TGameModel> gameAction, ref TGameModel game)
        {
            var result = _decoratee.PerformAction(gameAction, ref game);
            _eventEmitter.Trigger(_decoratee, new GameChange<TGameModel>(game, result));
            return result;
        }
    }
}
