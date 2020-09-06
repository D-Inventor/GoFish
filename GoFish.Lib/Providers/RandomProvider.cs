using System;

namespace GoFish.Lib.Providers
{
    internal class RandomProvider : IRandomProvider
    {
        private static readonly Lazy<RandomProvider> _lazyInstance = new Lazy<RandomProvider>(() => new RandomProvider());

        private readonly Random _instance;
        protected RandomProvider()
        {
            _instance = new Random();
        }

        public int Next(int min, int max) => _instance.Next(min, max);
        public int Next(int max) => _instance.Next(max);

        internal static IRandomProvider GetInstance() => _lazyInstance.Value;
    }
}
