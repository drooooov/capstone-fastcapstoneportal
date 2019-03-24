using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FASTCapstonePortal.Interfaces;
using FASTCapstonePortal.Model;
using FASTCapstonePortal.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FASTCapstonePortal.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class GroupsController : Controller
    {
        private readonly IGroup _groupService;
        private readonly IStudent _studentService;
        private readonly IProject _projectService;
        public GroupsController(IGroup groupService, IProject projectService, IStudent studentService)
        {
            _groupService = groupService;
            _projectService = projectService;
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IEnumerable<GroupResponse>> GetGroups()
        {
            var result = await _groupService.GetAllAsync();
            return result.Select(g => new GroupResponse(g)).ToList();
        }

        [HttpGet]
        public async Task<IEnumerable<GroupResponse>> GetGroupsWithoutProposedProjects()
        {
            var result = await _groupService.GetAllWithoutProposedProjectsAsync();
            return result.Select(g => new GroupResponse(g)).ToList();
        }

        [HttpGet]
        public async Task<IEnumerable<GroupResponse>> GetGroupsWithProposedProjects()
        {
            var result = await _groupService.GetAllWithProposedProjectsAsync();
            return result.Select(g => new GroupResponse(g)).ToList();
        }

        [HttpGet("{groupId:int}")]
        public async Task<IActionResult> GetGroupById([FromRoute] int groupId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Group g = await _groupService.GetByIdAsync(groupId);

            if (g == null)
            {
                return BadRequest("Group not found");
            }

            return Ok(new GroupResponse(g));
        }

        [HttpGet("{groupId:int}")]
        public async Task<IActionResult> GetProjectPreferences([FromRoute] int groupId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Group group = await _groupService.GetByIdAsync(groupId);
            if(group == null) return BadRequest("Group not found");
            if(!User.IsInRole("Admin"))
            {
                Student student = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));
                if(!student.Group.Equals(group)) return BadRequest("Action not allowed.");
            }
            var result = await _groupService.GetPreferencesInOrderAsync(groupId);
            return Ok(result.Select(p => new ProjectResponse(p)).ToList());
        }

        [HttpPut("{groupId:int}")]
        public async Task<IActionResult> UpdateDescription([FromRoute] int groupId, [FromBody][Required] string description)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Group group = await _groupService.GetByIdAsync(groupId);
            if (!User.IsInRole("Admin"))
            {
                Student student = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));

                if (!student.Group.Equals(group)) return BadRequest("You are not in this group");
                if (!student.GroupAdmin) return BadRequest("You are not group's admin");
            }
            await _groupService.UpdateDescriptionAsync(group, description, Int32.Parse(User.Identity.Name));
            return Ok();
        }

        [HttpPut("{groupId:int}")]
        public async Task<IActionResult> UpdateName([FromRoute] int groupId, [FromQuery][Required] string name)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Group group = await _groupService.GetByIdAsync(groupId);
            if (!User.IsInRole("Admin"))
            {
                Student student = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));

                if (!student.Group.Equals(group)) return BadRequest("You are not in this group");
                if (!student.GroupAdmin) return BadRequest("You are not group's admin");
            }
            await _groupService.UpdateNameAsync(group, name, Int32.Parse(User.Identity.Name));
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{groupId:int}")]
        public async Task<IActionResult> UnassignProject([FromRoute]int groupId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Group group = await _groupService.GetByIdAsync(groupId);
            if (group == null) return BadRequest("Group not found");
            await _groupService.UnAssignProjectAsync(group, Int32.Parse(User.Identity.Name));
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{groupId:int}")]
        public async Task<IActionResult> AssignProject([FromRoute]int groupId, [FromQuery][Required] int? projectId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Group group = await _groupService.GetByIdAsync(groupId);
            if (group == null) return BadRequest("Group not found");
            Project project = await _projectService.GetByIdAsync(projectId ?? 0);
            if (project == null) return BadRequest("Project not found");
            if (!project.Approved) return BadRequest("Approve the project first");
            if (project.AssignedGroup != null) return BadRequest("Project is already assigned to a group. Please unassign the project first.");
            if (group.FinalProjectAssigned != null) return BadRequest("Group already has a project assigned. Please unassign the project first.");
            await _groupService.AssignProjectAsync(project, group, Int32.Parse(User.Identity.Name));
            return Ok();
        }

        [HttpPost("{groupId:int}")]
        public async Task<IActionResult> AddProjectToPreference([FromQuery][Required]int? projectId, [FromRoute] int groupId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Project project = await _projectService.GetByIdAsync(projectId ?? 0);
            Group group = await _groupService.GetByIdAsync(groupId);
            if (project == null)
                return BadRequest("Project Not Found");
            if (group == null)
                return BadRequest("Group Not Found");

            if (!User.IsInRole("Admin"))
            {
                Student student = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));
                if (student.Group != group) return BadRequest("Action not allowed");
                if (!student.GroupAdmin) return BadRequest("You are not group admin");
            }
            if (await _groupService.ProjectExistsInPreferencesAsync(group, project)) return Ok("Already exists");
            await _groupService.AddToPreferencesAsync(project, group, Int32.Parse(User.Identity.Name));
            return Ok();
        }

        [HttpPost("{groupId:int}")]
        public async Task<IActionResult> RemoveProjectFromPreference([FromQuery][Required]int? projectId, [FromRoute] int groupId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Project project = await _projectService.GetByIdAsync(projectId ?? 0);
            Group group = await _groupService.GetByIdAsync(groupId);
            if (project == null)
                return BadRequest("Project Not Found");
            if (group == null)
                return BadRequest("Group Not Found");
            if (group.PreferredProjects.Where(p => p.ProjectId == projectId).First() == null)
                return BadRequest("Project is not in preferences");
            if (!User.IsInRole("Admin"))
            {
                Student student = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));
                if (student.Group != group) return BadRequest("Action not allowed");
                if (!student.GroupAdmin) return BadRequest("You are not group admin");
            }
            if (!await _groupService.ProjectExistsInPreferencesAsync(group, project)) return BadRequest("Project not in preferences");
            await _groupService.RemoveFromPreferencesAsync(project, group, Int32.Parse(User.Identity.Name));
            return Ok();
        }

        [HttpPost("{groupName}")]
        public async Task<IActionResult> CreateGroup([FromRoute] string groupName, [FromQuery][Required] int? groupAdminId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Group group = new Group { Name = groupName };
            if (!User.IsInRole("Admin"))
            {
                if (Int32.Parse(User.Identity.Name) != groupAdminId) return BadRequest("Action not allowed");
            }
            Student student = await _studentService.GetByIdAsync(groupAdminId ?? 0);
            if (student == null) return BadRequest("Student not found");
            if (student.Group != null) return BadRequest("Student is already in a group");
            student.GroupAdmin = true;
            student.Group = group;
            await _studentService.UpdateAsync(student);
            return Ok(new
            {
                groupId = group.Id
            });
        }
        [HttpPost("{groupId:int}")]
        public async Task<IActionResult> RankProjects([FromRoute] int groupId, [FromBody][Required] Dictionary<int, int> projects)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Group group = await _groupService.GetByIdAsync(groupId);
            int items = projects.Count;
            if (group == null)
                return BadRequest("Group not found");
            for (int i = 1; i <= items; i++)
            {
                if (!projects.ContainsKey(i)) return BadRequest("Invalid input");
                if (!await _projectService.ExistsAsync(projects[i])) return BadRequest("Invalid input");
            }
            await _groupService.UpdatePreferencesAsync(projects, group, Int32.Parse(User.Identity.Name));
            return Ok();

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> MatchProjects()
        {
            await _groupService.MatchGroupsProjectsAsync(Int32.Parse(User.Identity.Name));
            return Ok();
        }

        [HttpDelete("{groupId:int}")]
        public async Task<IActionResult> DeleteGroup([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Group group = await _groupService.GetByIdAsync(groupId);

            if (group == null) return BadRequest("Group not found");
            Student groupAdmin = group.Students.Where(s => s.GroupAdmin == true).FirstOrDefault();
            if(!User.IsInRole("Admin"))
            {
                Student student = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));
                if (!student.Group.Equals(groupAdmin)) return BadRequest("Action not allowed");
            }
            groupAdmin.GroupAdmin = false;
            await _studentService.UpdateAsync(groupAdmin);
            await _groupService.DeleteAsync(group, Int32.Parse(User.Identity.Name));
            return Ok();
        }
    }

}