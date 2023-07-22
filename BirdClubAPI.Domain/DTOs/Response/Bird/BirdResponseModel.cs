namespace BirdClubAPI.Domain.DTOs.Response.Bird
{
    public class BirdResponseModel
    {
        public string Name { get; set; } = null!;
        public string? Species { get; set; }
        public int Id { get; set; }
    }
}
