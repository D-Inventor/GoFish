using GoFish.Lib.Models;
using GoFish.Web.Models;

namespace GoFish.Web.Mappers
{
    public class CardMapper : IMapper<Card, CardViewModel>
    {
        public CardViewModel Map(Card input)
        {
            if (input is null) return null;

            CardViewModel result = new CardViewModel
            {
                Collection = input.Collection,
                Index = input.Index,
                Graphic = input.Description
            };

            return result;
        }
    }
}
