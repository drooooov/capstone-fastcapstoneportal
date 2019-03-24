using System;
using System.Threading.Tasks;
using FASTCapstonePortal.Interfaces;
using FASTCapstonePortal.Model;
using FASTCapstonePortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FASTCapstonePortal.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageWriterService _imageService;
        private readonly IStudent _studentService;
        private readonly IGroup _groupService;

        public ImageController(IImageWriterService imageService, IStudent studentService, IGroup groupService)
        {
            _imageService = imageService;
            _studentService = studentService;
            _groupService = groupService;
        }

        [HttpPost("{studentId:int}")]
        public async Task<IActionResult> UploadStudentImage(IFormFile file, [FromRoute] int studentId)
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
            try
            {
                Student result = await _studentService.GetByIdAsync(studentId);
                if (result == null) BadRequest("Student not found");

                string imgName = await _imageService.UploadImage(file);

                if (result.Picture != null) _imageService.DeleteImage(result.Picture);

                await _studentService.UploadStudentImageAsync(result, imgName);
                return Ok(imgName);
            }
            catch(Exception e)
            {
                ModelState.AddModelError("Error", e.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost("{groupId:int}")]
        public async Task<IActionResult> UploadGroupImage(IFormFile file, [FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _groupService.ExistsAsync(groupId)) return BadRequest("Group not found");
            if (!User.IsInRole("Admin"))
            {
                Student studentUser = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));

                if (studentUser.Group != null)
                {
                    if (studentUser.Group.Id != groupId)
                    {
                        return BadRequest("You are not in this group");
                    }
                    else if (!studentUser.GroupAdmin)
                    {
                        return BadRequest("You are not this group's admin");
                    }
                }
                else
                {
                    return BadRequest("You are not in any group");
                }
            }
            try
            {
                string imgName = await _imageService.UploadImage(file);
                Group result = await _groupService.GetByIdAsync(groupId);

                if (result.Picture != null)  _imageService.DeleteImage(result.Picture);

                await _groupService.UploadGroupImageAsync(result, imgName, Int32.Parse(User.Identity.Name));
                return Ok(imgName);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> TestGetImage()
        {
            Student student = await _studentService.GetByIdAsync(Int32.Parse(User.Identity.Name));

            return Ok(_imageService.ConvertFileToB64(student.Picture));
        }
    }
}