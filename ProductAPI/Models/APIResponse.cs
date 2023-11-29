namespace ProductAPI.Models
{
    public class APIResponse
    {
        public bool IsSucess { get; set; } = false;
        public string Error { get; set; }
        public object Result { get; set; }
        public string Message { get; set; }
    }
}
