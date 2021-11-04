namespace SecureAPI_Identity_EF.Models.Response
{
    // Using for custom response, we can use .NET Core Response Model also
    public class ResponseModel
    {
        public ResponseModel(ResponseCode responseCode, string message, object data)
        {
            ResponseCode = responseCode;
            Message = message;
            Data = data;
        }

        public ResponseCode ResponseCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}