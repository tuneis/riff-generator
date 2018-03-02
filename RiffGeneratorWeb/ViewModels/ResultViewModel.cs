namespace RiffGeneratorWeb.ViewModels
{
    public class ResultViewModel
    {
        public ResultViewModel()
        {

        }

        public ResultViewModel(Result outcome)
        {
            HasSucceeded = (outcome == Result.Success) ? true : false;
        }

        public ResultViewModel(object obj)
        {
            Data = obj;
        }

        public ResultViewModel(Result outcome, string message)
        {
            HasSucceeded = (outcome == Result.Success) ? true : false;
            Message = message;
        }

        public ResultViewModel(Result outcome, object obj)
        {
            HasSucceeded = (outcome == Result.Success) ? true : false;
            Data = obj;
        }

        public ResultViewModel(Result outcome, string message, object obj)
        {
            HasSucceeded = (outcome == Result.Success) ? true : false;
            Data = obj;
            Message = message;
        }

        public bool HasSucceeded { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public enum Result
    {
        Success = 0,
        Error = 1
    }
}
