namespace WebApi.Dto
{
    public class MakeOrderDto
    {
        public int UserId { get; set; }
        public List<int> ProductIds { get; set; }
    }
}
