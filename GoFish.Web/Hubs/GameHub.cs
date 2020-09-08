using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Game.Lib;

using GoFish.Lib.Models;
using GoFish.Web.Mappers;
using GoFish.Web.Models;
using GoFish.Web.Models.Events;
using GoFish.Web.Providers;
using GoFish.Web.Services;

using Microsoft.AspNetCore.SignalR;

namespace GoFish.Web.Hubs
{
    public class GameHub : Hub
    {
        private readonly IUserContextProvider _userContextProvider;
        private readonly IGameService _gameService;
        private readonly IMapper<GoFishGame, GameViewModel> _gameMapper;
        private readonly IMapper<Card, CardViewModel> _cardMapper;
        private readonly IAsyncEventEmitter<UserConnect<GameHub>> _userConnectEvent;
        private readonly IAsyncEventEmitter<UserDisconnect<GameHub>> _userDisconnectEvent;

        public GameHub(IUserContextProvider userContextProvider, IGameService gameService, IMapper<GoFishGame, GameViewModel> gameMapper, IMapper<Card, CardViewModel> cardMapper, IAsyncEventEmitter<UserConnect<GameHub>> userConnectEvent, IAsyncEventEmitter<UserDisconnect<GameHub>> userDisconnectEvent)
        {
            _userContextProvider = userContextProvider;
            _gameService = gameService;
            _gameMapper = gameMapper;
            _cardMapper = cardMapper;
            _userConnectEvent = userConnectEvent;
            _userDisconnectEvent = userDisconnectEvent;
        }

        public override Task OnConnectedAsync()
        {
            _userConnectEvent.Trigger(this, new UserConnect<GameHub>(Context.ConnectionId, _userContextProvider.UserId));
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _userDisconnectEvent.Trigger(this, new UserDisconnect<GameHub>(Context.ConnectionId));
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Initialise()
        {
            Guid userId = _userContextProvider.UserId;
            GoFishGame game = _gameService.Get();
            GameViewModel model = _gameMapper.Map(game);

            if (model != null)
            {
                Player player = game.Players.FirstOrDefault(p => p.Id.Equals(_userContextProvider.UserId));
                model.UserCards = !(player is null) ? _cardMapper.MapRange(player.Cards) : null;
            }

            await Clients.Caller.SendAsync("ReceiveGameData", userId, model);
        }

        public async Task Create(string Username)
        {
            try
            {
                Result result;
                if (!(result = _gameService.Create()).Success)
                {
                    await Clients.Caller.SendAsync("ReceiveError", result.Feedback);
                    return;
                }

                if (!(result = _gameService.Join(Username)).Success)
                {
                    await Clients.Caller.SendAsync("ReceiveError", result.Feedback);
                    return;
                }
            }
            catch (Exception e)
            {
                await Clients.Caller.SendAsync("ReceiveError", e.Message);
            }
        }

        public async Task Join(string Username)
        {
            try
            {
                Result result;
                if (!(result = _gameService.Join(Username)).Success)
                {
                    await Clients.Caller.SendAsync("ReceiveError", result.Feedback);
                    return;
                }
            }
            catch (Exception e)
            {
                await Clients.Caller.SendAsync("ReceiveError", e.Message);
            }
        }

        public async Task Start()
        {
            try
            {
                Result result;
                if (!(result = _gameService.Start()).Success)
                {
                    await Clients.Caller.SendAsync("ReceiveError", result.Feedback);
                    return;
                }
            }
            catch (Exception e)
            {
                await Clients.Caller.SendAsync("ReceiveError", e.Message);
            }
        }

        public async Task Pass(Guid id)
        {
            try
            {
                Result result;
                if (!(result = _gameService.Pass(id)).Success)
                {
                    await Clients.Caller.SendAsync("ReceiveError", result.Feedback);
                    return;
                }
            }
            catch (Exception e)
            {
                await Clients.Caller.SendAsync("ReceiveError", e.Message);
            }
        }

        public async Task Give(IEnumerable<CardViewModel> cards)
        {
            try
            {
                Result result;
                if (!(result = _gameService.Give(cards)).Success)
                {
                    await Clients.Caller.SendAsync("ReceiveError", result.Feedback);
                    return;
                }
            }
            catch (Exception e)
            {
                await Clients.Caller.SendAsync("ReceiveError", e.Message);
            }
        }
    }
}
