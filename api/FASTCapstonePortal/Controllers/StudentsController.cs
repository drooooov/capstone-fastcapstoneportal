using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FASTCapstonePortal.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using FASTCapstonePortal.ResponseModels;
using FASTCapstonePortal.RequestModels;
using System.ComponentModel.DataAnnotations;
using FASTCapstonePortal.Model;
using System;

namespace FASTCapstonePortal.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class StudentsController : Controller
    {

        private readonly IStudent _studentService;
        private readonly IGroup _groupService;
        private readonly UserManager<CapstoneUser> _userManager;

        public StudentsController(IStudent studentService, IGroup groupService, UserManager<CapstoneUser> userManager)
        {
            _studentService = studentService;
            _userManager = userManager;
            _groupService = groupService;
        }
        
        [HttpGet]
        public async Task<IEnumerable<StudentResponse>> GetAllStudents()
        {
            var result = await _studentService.GetAllAsync();
            return result.Select(s => new StudentResponse(s)).ToList();
        }

        [HttpGet("{groupId:int}")]
        public async Task<IActionResult> GetStudentsByGroupId([FromRoute] int groupId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _studentService.GetByGroupAsync(groupId);
            return Ok(result.Select(s => new StudentResponse(s)).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupAdmins()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _studentService.GetByGroupAdminAsync(true);
            return Ok(result.Select(s => new StudentResponse(s)).ToList());
        }

        [HttpGet("{studentId:int}")]
        public async Task<IActionResult> GetStudentById([FromRoute] int studentId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Student s = await _studentService.GetByIdAsync(studentId);
            if (s == null)
            {
                return BadRequest("Student not found");
            }
            return Ok(new StudentResponse(s));
        }

        [HttpGet("{prog:int}")]
        public async Task<IActionResult> GetStudentByProgram([FromRoute] int prog)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!Enum.IsDefined(typeof(Programs), prog)) return BadRequest("Course not found");
            Programs program = (Programs) prog ;
            var result = await _studentService.GetByProgramAsync(program);
            return Ok(result.Select(s => new StudentResponse(s)).ToList());
        }

        [HttpPut("{studentId:int}")]
        public async Task<IActionResult> UpdateStudent([FromBody][Required] StudentUpdate student, [FromRoute] int studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!User.IsInRole("Admin"))
            {
                if (User.Identity.Name != studentId.ToString())
                {
                    return BadRequest("Action not allowed");
                }
            }
            Student result = await _studentService.GetByIdAsync(studentId);
            if (result != null)
            {
                await _studentService.UpdateStudentThroughRequestModelAsync(result, student);
                return Ok();
            }
            return BadRequest("Student not found");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{studentId:int}")]
        public async Task<IActionResult> PutStudentInGroup([FromRoute] int studentId, [FromQuery][Required] int? groupId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Student student = await _studentService.GetByIdAsync(studentId);
            Group group = await _groupService.GetByIdAsync(groupId ?? 0);
            Student groupAdmin = group.Students.Where(s => s.GroupAdmin == true).FirstOrDefault();
            if (student == null || group == null) return BadRequest("Group or Student not found");
            if (student.Group != null) return BadRequest("Already in a group.");
            if (student.Program.Equals(groupAdmin.Program)) return BadRequest("Not in same program");
            await _studentService.PutStudentInGroupAsync(student, group, Int32.Parse(User.Identity.Name));
            return Ok();
        }

        [HttpPut("{studentId:int}")]
        public async Task<IActionResult> RemoveStudentFromGroup([FromRoute] int studentId, [FromQuery][Required] int? groupId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Student student = await _studentService.GetByIdAsync(studentId);
            Group group = await _groupService.GetByIdAsync(groupId ?? 0);
            if (student == null || group == null) return BadRequest("Group or Student not found");
            if (!group.Students.Any(s => s.Id == student.Id)) return BadRequest("Student is not in this group");
            if(student.GroupAdmin)
            {
                if (group.Students.Count > 1)
                {
                    return BadRequest("Group Admin not allowed to exit, make a student group admin before leaving.");
                }
                student.GroupAdmin = false;
                await _groupService.DeleteAsync(group, Int32.Parse(User.Identity.Name));
                return Ok();
            }
            await _studentService.RemoveStudentFromGroupAsync(student, groupId ?? 0, Int32.Parse(User.Identity.Name));
            return Ok();
        }


        [HttpPut("{groupAdminId:int}")]
        public async Task<IActionResult> ChangeGroupAdmin([FromRoute] int groupAdminId,[FromQuery][Required] int? newAdminId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Student admin = await _studentService.GetByIdAsync(groupAdminId);
            Student student = await _studentService.GetByIdAsync(newAdminId ?? 0);
            if (admin == null || student == null) return BadRequest("Student Not Found");
            Group group = admin.Group;
            if (group == null) return BadRequest("Group Not Found");
            if(admin.GroupAdmin)
            {
                if (!group.Students.Contains(student)) return BadRequest("Student is not part of the group");
                await _studentService.ChangeGroupAdminAsync(admin, student, Int32.Parse(User.Identity.Name));
                return Ok();
            }
            return BadRequest("Only the group admin can change the admin of the group");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{studentId:int}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Student student = await _studentService.GetByIdAsync(studentId);
            CapstoneUser user = student.User;
            if (student == null)
            {
                return BadRequest("Student not found");
            }

            await _studentService.DeleteAsync(student);
            await _userManager.DeleteAsync(user);
            return Ok();
        }

        
    }
}