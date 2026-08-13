using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.AdminReportDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminReportController : Controller
    {
        private readonly AppDbContext _context;

        public AdminReportController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? status)
        {
            var query = _context.MessageReports
                .AsNoTracking()
                .AsQueryable();

            query = status switch
            {
                "pending" =>
                    query.Where(report => !report.IsResolved),

                "resolved" =>
                    query.Where(report => report.IsResolved),

                _ => query
            };

            var reports = await query
                .OrderBy(report => report.IsResolved)
                .ThenByDescending(report => report.ReportDate)
                .Select(report => new AdminReportListDto
                {
                    Id = report.Id,
                    MessageId = report.MessageId,
                    Subject = report.Message.Subject,

                    SenderFullName =
                        report.Message.Sender.FirstName + " " +
                        report.Message.Sender.LastName,

                    ReportedByFullName =
                        report.ReportedByUser.FirstName + " " +
                        report.ReportedByUser.LastName,

                    Reason = report.Reason,
                    ReportDate = report.ReportDate,
                    IsResolved = report.IsResolved
                })
                .ToListAsync();

            ViewBag.Status = status;

            return View(reports);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var report = await _context.MessageReports
                .AsNoTracking()
                .Where(report => report.Id == id)
                .Select(report => new AdminReportDetailDto
                {
                    Id = report.Id,
                    MessageId = report.MessageId,
                    Reason = report.Reason,
                    Description = report.Description,
                    ReportDate = report.ReportDate,
                    IsResolved = report.IsResolved,

                    ReportedByFullName =
                        report.ReportedByUser.FirstName + " " +
                        report.ReportedByUser.LastName,

                    ReportedByEmail =
                        report.ReportedByUser.Email ?? string.Empty,

                    Subject = report.Message.Subject,
                    MessageBody = report.Message.Body,
                    MessageSendTime = report.Message.SendTime,

                    SenderId = report.Message.SenderId,

                    SenderFullName =
                        report.Message.Sender.FirstName + " " +
                        report.Message.Sender.LastName,

                    SenderEmail =
                        report.Message.Sender.Email ?? string.Empty,

                    ReceiverFullName =
                        report.Message.Receiver.FirstName + " " +
                        report.Message.Receiver.LastName,

                    ReceiverEmail =
                        report.Message.Receiver.Email ?? string.Empty
                })
                .FirstOrDefaultAsync();

            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resolve(int id)
        {
            var report = await _context.MessageReports.FindAsync(id);

            if (report == null)
            {
                return NotFound();
            }

            report.IsResolved = true;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Şikâyet çözüldü olarak işaretlendi.";

            return RedirectToAction(nameof(Detail), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reopen(int id)
        {
            var report = await _context.MessageReports.FindAsync(id);

            if (report == null)
            {
                return NotFound();
            }

            report.IsResolved = false;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Şikâyet yeniden incelemeye alındı.";

            return RedirectToAction(nameof(Detail), new { id });
        }
    }
}
