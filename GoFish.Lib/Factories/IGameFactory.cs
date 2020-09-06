using GoFish.Lib.Models;

namespace GoFish.Lib.Factories
{
    public interface IGameFactory
    {
        Models.GoFishGame Create(Settings settings);
    }
}
