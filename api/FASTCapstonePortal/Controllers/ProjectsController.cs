using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FASTCapstonePortal.Interfaces;
using FASTCapstonePortal.Model;
using FASTCapstonePortal.RequestModels;
using FASTCapstonePortal.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FASTCapstonePortal.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class ProjectsController : Controller
    {
        private readonly IProject _projectService;
        private readonly IStudent _studentService;
        private readonly IGroup _groupService;

        public ProjectsController(IProject projectService, IStudent studentService, IGroup groupService)
        {
            _projectService = projectService;
            _studentService = studentService;
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IEnumerable<ProjectResponse>> GetAllProjects()
        {
            var result = await _projectService.GetAllAsync();
            if (!User.IsInRole("Admin")) result = result.Where(p => p.Proposed == false);
            return result.Select(p => new ProjectResponse(p)).ToList();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<ProjectResponse>> GetApprovedProjects()
        {
            var result = await _projectService.GetApprovedProjectsAsync();
            return result.Select(p => new ProjectResponse(p)).ToList();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<ProjectResponse>> GetUnapprovedProjects()
        {
            var result = await _projectService.GetUnapprovedProjectsAsync();
            return result.Select(p => new ProjectResponse(p)).ToList();
        }

        [HttpGet("{clientName}")]
        public async Task<IEnumerable<ProjectResponse>> GetProjectByClient([FromRoute]string clientName)
        {
            var result = await _projectService.GetByClientAsync(clientName);
            if (!User.IsInRole("Admin")) result = result.Where(p => p.Proposed == false);
            return result.Select(p => new ProjectResponse(p)).ToList();
        }

        [HttpGet("{difficulty:int:max(5)}")]
        public async Task<IEnumerable<ProjectResponse>> GetProjectByDifficulty([FromRoute]int difficulty)
        {
            var result = await _projectService.GetByDifficultyAsync(difficulty);
            if (!User.IsInRole("Admin")) result = result.Where(p => p.Proposed == false);
            return result.Select(p => new ProjectResponse(p)).ToList();
        }

        [HttpGet("{projectId:int}")]
        public async Task<IActionResult> GetProjectById([FromRoute]int projectId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            int userId = Int32.Parse(User.Identity.Name);
            Project result = await _projectService.GetByIdAsync(projectId);
            if (result == null)
            {
                return BadRequest("Project not found");
            }
            if (!User.IsInRole("Admin") && result.Proposed)
            {
                Student student = await _studentService.GetByIdAsync(userId);
                if (student.Group.ProposedProject.Id != result.Id) result = null;
            }
            return Ok(new ProjectResponse(result));
        }

        [HttpGet("{ipType:int}")]
        public async Task<IEnumerable<ProjectResponse>> GetProjectByIpType([FromRoute]int ipType)
        {
            var result = await _projectService.GetByIpTypeAsync(ipType);
            if (!User.IsInRole("Admin")) result = result.Where(p => p.Proposed == false);
            return result.Select(p => new ProjectResponse(p)).ToList();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{projectId:int}")]
        public async Task<IEnumerable<GroupResponse>> GetGroupByPreferredProject([FromRoute]int projectId)
        {
            var result = await _projectService.GetGroupsByPreferredProjectAsync(projectId);
            return result.Select(g => new GroupResponse(g)).ToList();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{projectId:int}")]
        public async Task<IActionResult> UnApproveProject([FromRoute]int projectId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Project project = await _projectService.GetByIdAsync(projectId);
            if (project == null) return BadRequest("Project not found");
            if (project.AssignedGroup != null) return BadRequest("Unassign the group first");
            await _projectService.UnApproveProjectAsync(project, Int32.Parse(User.Identity.Name));
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{projectId:int}")]
        public async Task<IActionResult> ApproveProject([FromRoute]int projectId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Project project = await _projectService.GetByIdAsync(projectId);
            if (project == null) return BadRequest("Project not found");
            if (project.Approved) return Ok();
            await _projectService.ApproveProjectAsync(project, Int32.Parse(User.Identity.Name));
            return Ok();
        }
        
        [HttpPut("{projectId:int}")]
        public async Task<IActionResult> UpdateProject([FromBody][Required] ProjectUpdate project, [FromRoute] int projectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Project result = await _projectService.GetByIdAsync(projectId);
            if (result == null) return BadRequest("Project not found");

            if(!User.IsInRole("Admin"))
            {
                Student student = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));
                if (student.Group.ProposedProject == null || !student.Group.ProposedProject.Equals(result)) return BadRequest("Not allowed");
            }
            await _projectService.UpdateProjectThroughRequestModelAsync(result, project, Int32.Parse(User.Identity.Name));
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody][Required] ProjectCreate projectCreate)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Project project = projectCreate.NewProject();
            if (!User.IsInRole("Admin"))
            {
                Student student = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));
                if (!student.GroupAdmin) return BadRequest("You are not group admin.");
                project.Proposed = true;
                project.Approved = false;
                student.Group.ProposedProject = project;
                await _studentService.UpdateAsync(student);
                return Ok();
            }
            try
            {
                await _projectService.CreateAsync(project);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{projectId:int}")]
        public async Task<IActionResult> DeleteProject([FromRoute] int projectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Project project = await _projectService.GetByIdAsync(projectId);
            if (project == null)
            {
                return BadRequest("Project not found");
            }

            await _projectService.DeleteAsync(project);
            return Ok();
        }
    }
}