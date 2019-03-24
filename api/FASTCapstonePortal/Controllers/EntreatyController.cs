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
    [Authorize(Roles = "Student")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class EntreatyController : Controller
    {
        private readonly IStudent _studentService;
        private readonly IGroup _groupService;
        private readonly IEntreaty _entreatyService;

        public EntreatyController(IStudent studentService, IGroup groupService, IEntreaty entreatyService)
        {
            _studentService = studentService;
            _groupService = groupService;
            _entreatyService = entreatyService;
        }
        [HttpPost("{studentId:int}")]
        public async Task<IActionResult> CreateInviteAsync([FromRoute] int studentId, [FromBody][Required] string message)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Student groupAdmin = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));
            if (!groupAdmin.GroupAdmin) return BadRequest("Only group admins allowed");
            Student student = await _studentService.GetByIdAsync(studentId);
            if (student == null) return BadRequest("Student does not exist");
            if (!student.Program.Equals(groupAdmin.Program)) return BadRequest("Not in your program");
            if (student.Group != null) return BadRequest("Student is already part of a group");
            if (groupAdmin.Group.Students.Count >= 4) return BadRequest("More members not allowed");
            if (await _entreatyService.ExistsAsync(studentId, groupAdmin.Group.Id)) return BadRequest("A request/invite between this group and student already exists");
            Entreaty invite = new Entreaty()
            {
                Accepted = false,
                Group = groupAdmin.Group,
                Message = message,
                Student = student,
            };
            await _entreatyService.CreateInviteAsync(invite, groupAdmin.Id);
            return Ok();
        }

        [HttpPost("{groupId:int}")]
        public async Task<IActionResult> CreateRequestAsync([FromRoute] int groupId, [FromBody][Required] string message)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Student student = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));
            Group group = await _groupService.GetByIdAsync(groupId);
            if (group == null) return BadRequest("Group does not exist");
            Student groupAdmin = group.Students.Where(s => s.GroupAdmin == true).FirstOrDefault();
            if (!student.Program.Equals(groupAdmin.Program)) return BadRequest("Not in your program");
            if (student.Group != null) return BadRequest("You are already part of a group");
            if (group.Students.Count >= 4) return BadRequest("More members not allowed");
            if (await _entreatyService.ExistsAsync(student.Id, groupId)) return BadRequest("A request/invite between this group and student already exists");
            Entreaty request = new Entreaty()
            {
                Accepted = false,
                Group = group,
                Message = message,
                Student = student,
            };
            await _entreatyService.CreateRequestAsync(request, student.Id);
            return Ok();
         }

        [HttpPut("{groupId:int}")]
        public async Task<IActionResult> AcceptEntreaty([FromRoute] int groupId, [FromQuery][Required] int? studentId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Student user = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));
            Student student = await _studentService.GetByIdAsync(studentId ?? 0);
            if (student == null) return BadRequest("Student does not exist");
            Group group = await _groupService.GetByIdAsync(groupId);
            if (group == null) return BadRequest("Group does not exist");
            Student groupAdmin = group.Students.Where(s => s.GroupAdmin == true).First();
            
            //So we know it is one of the two parties
            if (user.Id != groupAdmin.Id && user.Id != student.Id) return BadRequest("Action not allowed");

            if (!await _entreatyService.ExistsAsync(studentId ?? 0, groupId)) return BadRequest("The entreaty does not exist");
            Entreaty entreaty = group.Entreaties.Where(e => e.StudentId == studentId).First();

            if (student.Group != null) return BadRequest("Student is already part of a group");
            if (group.Students.Count >= 4) return BadRequest("More members not allowed");

            if (entreaty.EntreatyType == EntreatyType.REQUEST && user.Id != groupAdmin.Id) return BadRequest("Only Group Admins allowed to accept");
            if (entreaty.EntreatyType == EntreatyType.INVITE && user.Id != student.Id) return BadRequest("Action not allowed");
            await _studentService.PutStudentInGroupAsync(entreaty.Student, entreaty.Group, user.Id);
            return Ok();
        }

        [HttpPut("{groupId:int}")]
        public async Task<IActionResult> RejectEntreaty([FromRoute] int groupId, [FromQuery][Required] int? studentId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Student user = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));
            Student student = await _studentService.GetByIdAsync(studentId ?? 0);
            if (student == null) return BadRequest("Student does not exist");
            Group group = await _groupService.GetByIdAsync(groupId);
            if (group == null) return BadRequest("Group does not exist");
            Student groupAdmin = group.Students.Where(s => s.GroupAdmin == true).First();

            if (user.Id != groupAdmin.Id && user.Id != student.Id) return BadRequest("Action not allowed");

            if (!await _entreatyService.ExistsAsync(studentId ?? 0, groupId)) return BadRequest("The entreaty does not exist");
            Entreaty entreaty = group.Entreaties.Where(e => e.StudentId == studentId).First();

            if (student.Group != null) return BadRequest("Student is already part of a group");
            if (group.Students.Count >= 4) return BadRequest("More members not allowed");

            if (entreaty.EntreatyType == EntreatyType.REQUEST && user.Id != groupAdmin.Id) return BadRequest("Only Group Admins allowed to reject");
            if (entreaty.EntreatyType == EntreatyType.INVITE && user.Id != student.Id) return BadRequest("Action not allowed");

            await _entreatyService.RejectAsync(entreaty);
            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<EntreatyResponse>> GetStudentEntreaties()
        {
            Student student = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));
            return student.Entreaties.Select(e => new EntreatyResponse(e)).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupEntreaties()
        {
            Student student = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));
            if (student.Group == null) return BadRequest("You are not in a group.");
            return Ok(student.Group.Entreaties.Select(e => new EntreatyResponse(e)).ToList());
        }
    }
}