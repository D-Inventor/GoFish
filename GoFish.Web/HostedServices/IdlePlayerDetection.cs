using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Game.Lib;

using GoFish.Lib.GameActions;
using GoFish.Lib.Models;
using GoFish.Web.Models.Events;
using GoFish.Web.Services;

using Microsoft.Extensions.Hosting;

namespace GoFish.Web.HostedServices
{
    public class IdlePlayerDetection : IHostedService, IDisposable
    {
        private readonly IAsyncEventEmitter<UserActivity> _userActivityEvent;
        private readonly IGameManager<GoFishGame> _gameManager;
        private readonly IGameAccessor _gameAccessor;
        private Timer _timer;

        private static readonly ConcurrentDictionary<Guid, DateTimeOffset> _lastRegisteredActivity = new ConcurrentDictionary<Guid, DateTimeOffset>();

        public IdlePlayerDetection(IAsyncEventEmitter<UserActivity> userActivityEvent, IGameManager<GoFishGame> gameManager, IGameAccessor gameAccessor)
        {
            _userActivityEvent = userActivityEvent;
            _gameManager = gameManager;
            _gameAccessor = gameAccessor;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _userActivityEvent.OnEvent.Subscribe(OnUserActivity);

            _timer = new Timer(CheckIdle, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private void CheckIdle(object state)
        {
            var game = _gameAccessor.Game;
            if (game is null) return;
            foreach (var key in _lastRegisteredActivity.Keys)
            {
                if (_lastRegisteredActivity.TryGetValue(key, out var dateTime) && (DateTimeOffset.UtcNow - dateTime).TotalMinutes >= 5)
                {
                    if (!_lastRegisteredActivity.TryRemove(key, out _)) continue;

                    var player = game.Players.FirstOrDefault(p => p.Id.Equals(key));
                    if (player is null) continue;

                    var gameAction = new LeaveGameAction(player);

                    _gameManager.PerformAction(gameAction, ref game);
                }
            }
        }

        private Task OnUserActivity(object sender, UserActivity args)
        {
            _lastRegisteredActivity.AddOrUpdate(args.UserId, DateTimeOffset.UtcNow, (_, _1) => DateTimeOffset.UtcNow);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _userActivityEvent.OnEvent.Unsubscribe(OnUserActivity);
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
