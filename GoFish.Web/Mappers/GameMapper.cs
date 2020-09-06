
using GoFish.Lib.Models;
using GoFish.Web.Models;

namespace GoFish.Web.Mappers
{
    public class GameMapper : IMapper<GoFishGame, GameViewModel>
    {
        private readonly IMapper<Player, PlayerViewModel> _playerMapper;

        public GameMapper(IMapper<Player, PlayerViewModel> playerMapper)
        {
            _playerMapper = playerMapper;
        }

        public GameViewModel Map(GoFishGame input)
        {
            if (input is null) return null;

            GameViewModel result = new GameViewModel
            {
                RuleSet = input.RuleSet,
                CurrentPlayer = input.CurrentPlayerIndex,
                NextPlayer = input.NextPlayerIndex,
                Players = _playerMapper.MapRange(input.Players),
                Deck = input.Deck.Count,
                GameState = input.GameState
            };

            return result;
        }
    }
}
