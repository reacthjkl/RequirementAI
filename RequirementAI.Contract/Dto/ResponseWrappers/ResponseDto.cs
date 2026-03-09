namespace RequirementAI.Contract.Dto.ResponseWrappers;

public class ResponseDto<T>
{
    public bool Successful { get; set; }
    public string? Message { get; set; } = null;
    public T? Data { get; set; }

    public static ResponseDto<T> Success(T data)
    {
        return new ResponseDto<T>
        {
            Successful = true,
            Data = data
        };
    }

    public static ResponseDto<List<T>> SuccessList(IEnumerable<T> data)
    {
        return new ResponseDto<List<T>>
        {
            Successful = true,
            Data = data.ToList()
        };
    }
}

public class ResponseDto
{
    public bool Successful { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }


    public static ResponseDto Success()
    {
        return new ResponseDto
        {
            Successful = true
        };
    }

    public static ResponseDto SuccessMessage(string message)
    {
        return new ResponseDto
        {
            Successful = true,
            Message = message
        };
    }

    public static ResponseDto Fail(string errorMessage)
    {
        return new ResponseDto
        {
            Successful = false,
            Message = errorMessage
        };
    }
}