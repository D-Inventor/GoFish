namespace GoFish.Lib.Providers
{
    public interface IRandomProvider
    {
        int Next(int max);
        int Next(int min, int max);
    }
}