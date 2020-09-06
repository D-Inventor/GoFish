using GoFish.Lib.Models;

namespace GoFish.Lib.Factories
{
    public class GameFactory : IGameFactory
    {
        private readonly IDeckFactory _deckFactory;

        public GameFactory(IDeckFactory deckFactory)
        {
            _deckFactory = deckFactory;
        }

        public GoFishGame Create(Settings settings)
        {
            GoFishGame result = new GoFishGame
            {
                RuleSet = settings.RuleSet,
                Deck = _deckFactory.Create(settings.DeckSettings)
            };

            return result;
        }
    }
}
