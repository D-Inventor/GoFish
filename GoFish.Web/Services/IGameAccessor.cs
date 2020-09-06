using GoFish.Lib.Models;

namespace GoFish.Web.Services
{
    public interface IGameAccessor
    {
        GoFishGame Game { get; set; }
    }
}