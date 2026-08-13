using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.AdminMessageDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminMessageController : Controller
    {
        private readonly AppDbContext _context;

        public AdminMessageController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? search,
            string? status)
        {
            var query = _context.UserMessages
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(message =>
                    message.Subject.Contains(search) ||
                    message.Sender.FirstName.Contains(search) ||
                    message.Sender.LastName.Contains(search) ||
                    (message.Sender.Email != null &&
                     message.Sender.Email.Contains(search)) ||
                    message.Receiver.FirstName.Contains(search) ||
                    message.Receiver.LastName.Contains(search) ||
                    (message.Receiver.Email != null &&
                     message.Receiver.Email.Contains(search)));
            }

            query = status switch
            {
                "read" =>
                    query.Where(message => message.IsRead),

                "unread" =>
                    query.Where(message => !message.IsRead),

                "important" =>
                    query.Where(message => message.IsImportant),

                "reported" =>
                    query.Where(message =>
                        message.MessageReports.Any()),

                "trash" =>
                    query.Where(message =>
                        (message.IsTrashedBySender &&
                         !message.IsPermanentlyDeletedBySender)
                        ||
                        (message.IsTrashedByReceiver &&
                         !message.IsPermanentlyDeletedByReceiver)),

                _ => query
            };

            var messages = await query
                .OrderByDescending(message => message.SendTime)
                .Select(message => new AdminMessageListDto
                {
                    Id = message.Id,
                    Subject = message.Subject,

                    SenderFullName =
                        message.Sender.FirstName + " " +
                        message.Sender.LastName,

                    SenderEmail =
                        message.Sender.Email ?? string.Empty,

                    ReceiverFullName =
                        message.Receiver.FirstName + " " +
                        message.Receiver.LastName,

                    ReceiverEmail =
                        message.Receiver.Email ?? string.Empty,

                    SendTime = message.SendTime,
                    IsRead = message.IsRead,
                    IsImportant = message.IsImportant,

                    IsInTrash =
                        (message.IsTrashedBySender &&
                         !message.IsPermanentlyDeletedBySender)
                        ||
                        (message.IsTrashedByReceiver &&
                         !message.IsPermanentlyDeletedByReceiver),

                    ReportCount =
                        message.MessageReports.Count
                })
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Status = status;

            return View(messages);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var message = await _context.UserMessages
                .AsNoTracking()
                .Where(message => message.Id == id)
                .Select(message => new AdminMessageDetailDto
                {
                    Id = message.Id,
                    Subject = message.Subject,
                    Body = message.Body,
                    SendTime = message.SendTime,
                    IsRead = message.IsRead,
                    IsImportant = message.IsImportant,

                    SenderFullName =
                        message.Sender.FirstName + " " +
                        message.Sender.LastName,

                    SenderEmail =
                        message.Sender.Email ?? string.Empty,

                    SenderImageUrl =
                        message.Sender.ProfileImageUrl,

                    ReceiverFullName =
                        message.Receiver.FirstName + " " +
                        message.Receiver.LastName,

                    ReceiverEmail =
                        message.Receiver.Email ?? string.Empty,

                    ReceiverImageUrl =
                        message.Receiver.ProfileImageUrl,

                    IsTrashedBySender =
                        message.IsTrashedBySender,

                    IsTrashedByReceiver =
                        message.IsTrashedByReceiver,

                    ReportCount =
                        message.MessageReports.Count
                })
                .FirstOrDefaultAsync();

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }
    }
}