using System.Collections.Generic;

namespace Game.Lib
{
    public class ResultBuilder
    {
        private readonly List<string> _feedback;
        private bool _success;

        public ResultBuilder()
        {
            _feedback = new List<string>();
            _success = true;
        }

        public ResultBuilder AddFeedback(string feedback, bool success = true)
        {
            _feedback.Add(feedback);
            _success = _success && success;
            return this;
        }

        public ResultBuilder AddResult(Result result)
        {
            _feedback.AddRange(result.Feedback);
            _success = _success && result.Success;
            return this;
        }

        public Result Build()
        {
            return Result.Create(_success, _feedback);
        }
    }
}
