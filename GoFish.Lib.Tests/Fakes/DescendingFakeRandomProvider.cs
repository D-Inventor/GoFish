
using GoFish.Lib.Providers;

namespace GoFish.Lib.Tests.Fakes
{
    internal class DescendingFakeRandomProvider : IRandomProvider
    {
        private int nextValue;

        public DescendingFakeRandomProvider(int startValue)
        {
            nextValue = startValue;
        }

        public int Next(int max)
        {
            return nextValue--;
        }

        public int Next(int min, int max)
        {
            return nextValue--;
        }
    }
}
