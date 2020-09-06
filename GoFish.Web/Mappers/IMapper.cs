using System.Collections.Generic;
using System.Linq;

namespace GoFish.Web.Mappers
{
    public interface IMapper<in TInput, out TOutput>
    {
        TOutput Map(TInput input);
    }

    public static class IMapperExtensions
    {
        public static IEnumerable<TOut> MapRange<TIn, TOut>(this IMapper<TIn, TOut> mapper, IEnumerable<TIn> values)
        {
            return values.Select(v => mapper.Map(v));
        }
    }
}
