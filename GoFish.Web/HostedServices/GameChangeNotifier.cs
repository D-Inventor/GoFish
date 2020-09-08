using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Game.Lib;

using GoFish.Lib.Models;
using GoFish.Web.Hubs;
using GoFish.Web.Mappers;
using GoFish.Web.Models;
using GoFish.Web.Models.Events;
using GoFish.Web.Services;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace GoFish.Web.HostedServices
{
    public class GameChangeNotifier : IHostedService
    {
        private readonly IHubContext<GameHub> _gameHub;
        private readonly IMapper<GoFishGame, GameViewModel> _gameMapper;
        private readonly IMapper<Card, CardViewModel> _cardMapper;
        private readonly IAsyncEventEmitter<GameChange<GoFishGame>> _gamechangeEvent;
        private readonly IAsyncEventEmitter<UserConnect<GameHub>> _userconnectEvent;
        private readonly IAsyncEventEmitter<UserDisconnect<GameHub>> _userdisconnectEvent;

        private static readonly ConcurrentDictionary<string, Guid> _connectedUsers = new ConcurrentDictionary<string, Guid>();

        public GameChangeNotifier(IHubContext<GameHub> gameHub, IMapper<GoFishGame, GameViewModel> gameMapper, IMapper<Card, CardViewModel> cardMapper, IAsyncEventEmitter<GameChange<GoFishGame>> gamechangeEvent, IAsyncEventEmitter<UserConnect<GameHub>> userconnectEvent, IAsyncEventEmitter<UserDisconnect<GameHub>> userdisconnectEvent)
        {
            _gameHub = gameHub;
            _gameMapper = gameMapper;
            _cardMapper = cardMapper;
            _gamechangeEvent = gamechangeEvent;
            _userconnectEvent = userconnectEvent;
            _userdisconnectEvent = userdisconnectEvent;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _gamechangeEvent.OnEvent.Subscribe(GamechangeEvent_OnEvent);
            _userconnectEvent.OnEvent.Subscribe(UserconnectEvent_OnEvent);
            _userdisconnectEvent.OnEvent.Subscribe(UserdisconnectEvent_OnEvent);
            return Task.CompletedTask;
        }

        private Task UserconnectEvent_OnEvent(object sender, UserConnect<GameHub> e)
        {
            _connectedUsers.TryAdd(e.ConnectionId, e.UserId);
            return Task.CompletedTask;
        }

        private Task UserdisconnectEvent_OnEvent(object sender, UserDisconnect<GameHub> e)
        {
            _connectedUsers.TryRemove(e.ConnectionId, out _);
            return Task.CompletedTask;
        }

        private Task GamechangeEvent_OnEvent(object sender, GameChange<GoFishGame> e)
        {
            return SendGameChange(e.Result, e.Game);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _gamechangeEvent.OnEvent.Unsubscribe(GamechangeEvent_OnEvent);
            _userconnectEvent.OnEvent.Unsubscribe(UserconnectEvent_OnEvent);
            _userdisconnectEvent.OnEvent.Unsubscribe(UserdisconnectEvent_OnEvent);
            return Task.CompletedTask;
        }

        private async Task SendGameChange(Result result, GoFishGame game)
        {
            GameViewModel model = _gameMapper.Map(game);
            foreach (KeyValuePair<string, Guid> user in _connectedUsers)
            {
                Player player = game.Players.FirstOrDefault(p => p.Id.Equals(user.Value));
                model.UserCards = !(player is null) ? _cardMapper.MapRange(player.Cards) : null;
                await _gameHub.Clients.Client(user.Key).SendAsync("ReceiveGameChange", result, model);
            }
        }
    }
}
