namespace RentCar.Application.Helpers.GenerateJWT
{
    public class ApiResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResult<T> Success(T data) => new ApiResult<T>
        {
            IsSuccess = true,
            Data = data
        };

        public static ApiResult<T> Failure(IEnumerable<string> errors) => new ApiResult<T>
        {
            IsSuccess = false,
            Errors = errors.ToList()
        };

        public static ApiResult<T> Failure(string error) => new ApiResult<T>
        {
            IsSuccess = false,
            Errors = new List<string> { error }
        };
    }

}
