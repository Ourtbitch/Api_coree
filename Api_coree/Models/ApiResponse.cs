namespace Api_coree.Models
{
    public class ApiResponse<T>
    {
        public string mensaje { get; set; }
        public T Response { get; set; }
    }

}
