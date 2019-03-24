using FASTCapstonePortal.Interfaces;
using FASTCapstonePortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace FASTCapstonePortal.Controllers
{
    [Authorize(Roles="Admin")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]

    public class SeedController : Controller
    {
        
        private readonly IGroup _groupService;
        private readonly IProject _projectService;
        private readonly IImageWriterService _imageService;

        private readonly UserManager<CapstoneUser> _userManager;
        public SeedController(IGroup groupService, IProject projectService, UserManager<CapstoneUser> userManager, IImageWriterService imageService)
        {
            _groupService = groupService;
            _projectService = projectService;
            _userManager = userManager;
            _imageService = imageService;
        }

        [HttpGet]
        public IEnumerable GetAllUsernames()
        {
            return _userManager.Users.Select(u => u.UserName);
        }

        [HttpPut]
        public async Task<IActionResult> RankProjectsRandomly()
        {
            await _groupService.RankProjectsRandomlyAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UnassignAllProjects()
        {
            await _projectService.UnassignAllProjectsAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllImages()
        {
            await _imageService.DeleteAllImages();
            return Ok();
        }
    }
}