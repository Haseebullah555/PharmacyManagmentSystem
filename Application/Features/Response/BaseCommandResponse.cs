namespace Application.Features.Response
{
    public class BaseCommandResponse
    {
        public BaseCommandResponse()
        {
            
        }
        public int? Id { get; set; }
        public bool? Success { get; set; }
        public string Message { get; set; }
        public BaseCommandResponse(string? message, int? id, bool? success = true)
        {
            Id = id;
            Success = success;
            Message = message;
        }
    }

}