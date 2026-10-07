using UniversalTechnicalTest.Api.DTOs.Posts;

namespace UniversalTechnicalTest.Api.Services.Interfaces
{
    public interface IPostsService
    {
        Task<IEnumerable<PostResponse>> GetPostAsync();

        Task<PostResponse?> CreatePostAsync (CreatePostRequest postRequest);
    }
}
