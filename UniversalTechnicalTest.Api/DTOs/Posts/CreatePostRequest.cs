namespace UniversalTechnicalTest.Api.DTOs.Posts
{
    public class CreatePostRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public int UserId { get; set; }
        }
}
