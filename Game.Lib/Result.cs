using System.Collections.Generic;
using System.Linq;

namespace Game.Lib
{
    public class Result
    {
        protected Result(bool success, IReadOnlyList<string> feedback)
        {
            Success = success;
            Feedback = feedback;
        }

        public bool Success { get; }
        public IReadOnlyList<string> Feedback { get; }

        internal static Result Create(bool success, IEnumerable<string> feedback = null)
        {
            return new Result(success, feedback?.ToList() ?? new List<string>());
        }
    }
}
