using UniversalTechnicalTest.Api.DTOs.Posts;
using UniversalTechnicalTest.Api.Services.Interfaces;

namespace UniversalTechnicalTest.Api.Services
{
    public class PostsService : IPostsService
    {
        private readonly HttpClient _httpClient;

        public PostsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<PostResponse?> CreatePostAsync(CreatePostRequest postRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("posts", postRequest);

            return await response.Content.ReadFromJsonAsync<PostResponse>();
        }

        public async Task<IEnumerable<PostResponse>> GetPostAsync()
        {
            var posts = await _httpClient.GetFromJsonAsync<IEnumerable<PostResponse>>("posts");

            return posts ?? [];

        }


    }
}
