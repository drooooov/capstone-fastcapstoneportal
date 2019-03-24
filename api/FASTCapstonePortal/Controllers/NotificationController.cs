using FASTCapstonePortal.Interfaces;
using FASTCapstonePortal.Model;
using FASTCapstonePortal.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FASTCapstonePortal.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class NotificationController : Controller
    {
        private readonly IStudent _studentService;
        private readonly IGroup _groupService;
        private readonly INotification _notificationService;

        public NotificationController(IStudent studentService, IGroup groupService, INotification notificationService)
        {
            _studentService = studentService;
            _groupService = groupService;
            _notificationService = notificationService;
        }

        [HttpGet("{numOfWeeks:int}")]
        public async Task<IEnumerable<NotificationResponse>> GetAllPastWeeksNotifications([FromRoute] int numOfWeeks)
        {
            return (await _notificationService.GetAllReceivedNotificationsInPastWeekAsync(Int32.Parse(User.Identity.Name), numOfWeeks))
                .Select(n => new NotificationResponse(n)).ToList();
        }

        [HttpGet("{numOfWeeks:int}")]
        public async Task<IEnumerable<NotificationResponse>> GetAllUnreadNotifications([FromRoute] int numOfWeeks)
        {
            return (await _notificationService.GetAllUnreadNotificationsAsync(Int32.Parse(User.Identity.Name), numOfWeeks))
                .Select(n => new NotificationResponse(n)).ToList();
        }

        [HttpGet("{numOfWeeks:int}")]
        public async Task<IEnumerable<NotificationResponse>> GetAllReadNotifications([FromRoute] int numOfWeeks)
        {
            return (await _notificationService.GetAllReadNotificationsAsync(Int32.Parse(User.Identity.Name), numOfWeeks))
                .Select(n => new NotificationResponse(n)).ToList();
        }

        [HttpGet]
        public async Task<IEnumerable<AnnouncementResponse>> GetAllAnnouncementsAsync()
        {
            return (await _notificationService.GetAllAnnouncementsAsync()).Select(a => new AnnouncementResponse(a)).ToList();
        }

        [HttpPut]
        public async Task<IActionResult> MarkNotificationsAsRead([FromBody][Required] IEnumerable<int> notificationIds)
        {
            foreach (int notificationId in notificationIds)
            {
                Notification notification = await _notificationService.GetByIdAsync(notificationId);
                if (notification != null && notification.ReceiverId == Int32.Parse(User.Identity.Name))
                {
                    await _notificationService.ReadNotificationAync(notification);
                }
            }
            return Ok();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> MakeAnnouncement([FromBody][Required] string message)
        {
            await _notificationService.AnnounceToAllStudentsAsync(message, Int32.Parse(User.Identity.Name));
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{notificationContextId:int}")]
        public async Task<IActionResult> DeleteAnnouncement([FromRoute] int notificationContextId)
        {
            NotificationContext notificationContext = await _notificationService.GetAnnouncementByIdAsync(notificationContextId);
            if (notificationContext == null) return BadRequest("Announcement not found");
            await _notificationService.DeleteAnnouncementAsync(notificationContext);
            return Ok();
        }
    }
}