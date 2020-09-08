using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace GoFish.Web.Factories
{
    public class KeyFactory : IKeyFactory
    {
        public KeyFactory(RNGCryptoServiceProvider cryptoServiceProvider)
        {
            _cryptoServiceProvider = cryptoServiceProvider;
        }

        private static readonly ConcurrentDictionary<string, byte[]> _keycollection = new ConcurrentDictionary<string, byte[]>();
        private readonly RNGCryptoServiceProvider _cryptoServiceProvider;

        public byte[] Create(string name)
            => _keycollection.GetOrAdd(name, CreateNew);

        private byte[] CreateNew(string name)
        {
            byte[] key = new byte[16];
            _cryptoServiceProvider.GetBytes(key);
            return key;
        }
    }
}
