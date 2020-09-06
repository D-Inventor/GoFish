using GoFish.Lib.Models;

namespace GoFish.Lib.Factories
{
    public interface IDeckFactory
    {
        Deck Create(DeckSettings settings);
    }
}
