using System.Collections.Generic;

namespace UrbanRFP.Models
{
    public class AppResponseModel<T>
    {
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; }
        public string DeveloperMessage { get; set; }
        public T Data { get; set; }
        public List<T> DataCollection { get; set; }
    }
}