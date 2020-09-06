using System.Collections.Generic;
using System.Linq;

using GoFish.Lib.Models;
using GoFish.Web.Models;

namespace GoFish.Web.Mappers
{
    public class PlayerMapper : IMapper<Player, PlayerViewModel>
    {
        private readonly IMapper<Card, CardViewModel> _cardMapper;

        public PlayerMapper(IMapper<Card, CardViewModel> cardMapper)
        {
            _cardMapper = cardMapper;
        }

        public PlayerViewModel Map(Player input)
        {
            if (input is null) return null;

            Dictionary<string, List<CardViewModel>> finishedCollections = new Dictionary<string, List<CardViewModel>>(input.FinishedCollections.Select(s => KeyValuePair.Create(s.Key, _cardMapper.MapRange(s.Value).ToList())));

            PlayerViewModel result = new PlayerViewModel
            {
                Cards = input.Cards.Count,
                Id = input.Id,
                Username = input.Username,
                FinishedCollections = finishedCollections
            };

            return result;
        }
    }
}
