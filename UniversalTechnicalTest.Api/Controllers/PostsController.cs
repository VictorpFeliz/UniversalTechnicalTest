using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversalTechnicalTest.Api.Services.Interfaces;

namespace UniversalTechnicalTest.Api.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _postsService;

        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postsService.GetPostAsync();
            
            return Ok(posts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(DTOs.Posts.CreatePostRequest postRequest)
        {
            var post = await _postsService.CreatePostAsync(postRequest);
            
            return Ok(post);
        }
    }
}
