using System;
using System.Collections.Generic;
using System.Linq;

using Game.Lib;

using GoFish.Lib.Factories;
using GoFish.Lib.GameActions;
using GoFish.Lib.Models;
using GoFish.Web.Models;
using GoFish.Web.Providers;

namespace GoFish.Web.Services
{
    public class GameService : IGameService
    {
        private readonly IGameAccessor _gameAccessor;
        private readonly IGameFactory _gameFactory;
        private readonly IGameManager<GoFishGame> _gameManager;
        private readonly IUserContextProvider _userContextProvider;

        public GameService(IGameAccessor gameAccessor, IGameFactory gameFactory, IGameManager<GoFishGame> gameManager, IUserContextProvider userContextProvider)
        {
            _gameAccessor = gameAccessor;
            _gameFactory = gameFactory;
            _gameManager = gameManager;
            _userContextProvider = userContextProvider;
        }

        public GoFishGame Get()
        {
            return _gameAccessor.Game;
        }

        public Result Create()
        {
            ResultBuilder resultBuilder = new ResultBuilder();
            if (_gameAccessor.Game != null && _gameAccessor.Game.GameState != GameState.Finished)
            {
                resultBuilder.AddFeedback("Cannot create a new game when there's still a game active.", false);
                return resultBuilder.Build();
            }

            _gameAccessor.Game = _gameFactory.Create(new Settings
            {
                RuleSet = new RuleSet
                {
                    CardsOnStart = 4,
                    GiveCardBehaviour = GiveCardBehaviour.Single,
                    StartBehaviour = StartBehaviour.RandomPlayer,
                    TurnBehaviour = TurnBehaviour.LastAskedPlayer
                },
                DeckSettings = new DeckSettings()
            });

            resultBuilder.AddFeedback("A new game was created.");

            return resultBuilder.Build();
        }

        public Result Join(string username)
        {
            JoinGameAction gameAction = new JoinGameAction(_userContextProvider.UserId, username);

            GoFishGame game = _gameAccessor.Game;
            Result result = _gameManager.PerformAction(gameAction, ref game);
            return result;
        }

        public Result Start()
        {
            StartGameAction gameAction = new StartGameAction();

            GoFishGame game = _gameAccessor.Game;
            Result result = _gameManager.PerformAction(gameAction, ref game);
            return result;
        }

        public Result Pass(Guid id)
        {
            GoFishGame game = _gameAccessor.Game;

            Guid fromGuid = _userContextProvider.UserId;
            Player from = game.Players.FirstOrDefault(p => p.Id.Equals(fromGuid));
            Player to = game.Players.FirstOrDefault(p => p.Id.Equals(id));

            PassTurnGameAction gameAction = new PassTurnGameAction(from, to);

            Result result = _gameManager.PerformAction(gameAction, ref game);
            return result;
        }

        public Result Give(IEnumerable<CardViewModel> cards)
        {
            GoFishGame game = _gameAccessor.Game;
            Guid sourceGuid = _userContextProvider.UserId;
            Player sourcePlayer = game.Players.FirstOrDefault(p => p.Id.Equals(sourceGuid));

            HashSet<Card> cardset = new HashSet<Card>(cards.Select(c1 => sourcePlayer.Cards.FirstOrDefault(c => c.Collection.Equals(c1.Collection) && c.Index.Equals(c1.Index))));

            GiveCardsGameAction gameAction = new GiveCardsGameAction(cardset, sourcePlayer);

            Result result = _gameManager.PerformAction(gameAction, ref game);
            return result;
        }
    }
}
