using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.AdminDashboardDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AdminDashboardController(
            AppDbContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var model = new AdminDashboardDto
            {
                TotalUserCount = await _userManager.Users
                    .CountAsync(),

                ActiveUserCount = await _userManager.Users
                    .CountAsync(user => user.IsActive),

                TotalMessageCount = await _context.UserMessages
                    .CountAsync(),

                TodayMessageCount = await _context.UserMessages
                    .CountAsync(message =>
                        message.SendTime >= today &&
                        message.SendTime < tomorrow),

                UnreadMessageCount = await _context.UserMessages
                    .CountAsync(message => !message.IsRead),

                TrashMessageCount = await _context.UserMessages
                    .CountAsync(message =>
                        (message.IsTrashedBySender &&
                         !message.IsPermanentlyDeletedBySender)
                        ||
                        (message.IsTrashedByReceiver &&
                         !message.IsPermanentlyDeletedByReceiver)),

                PendingReportCount = await _context.MessageReports
                    .CountAsync(report => !report.IsResolved)
            };

            model.TopSenders = await _context.UserMessages
                .AsNoTracking()
                .Where(message =>
                    !message.IsPermanentlyDeletedBySender)
                .GroupBy(message => new
                {
                    message.SenderId,
                    message.Sender.FirstName,
                    message.Sender.LastName,
                    message.Sender.ProfileImageUrl
                })
                .Select(group => new TopSenderDto
                {
                    FullName =
                        group.Key.FirstName + " " +
                        group.Key.LastName,

                    ImageUrl =
                        group.Key.ProfileImageUrl,

                    MessageCount =
                        group.Count()
                })
                .OrderByDescending(user =>
                    user.MessageCount)
                .Take(5)
                .ToListAsync();

            var categoryStatistics =
                await _context.UserMessageCategories
                    .AsNoTracking()
                    .Where(item => item.Category.IsActive)
                    .GroupBy(item => new
                    {
                        item.CategoryId,
                        item.Category.Name,
                        item.Category.Color
                    })
                    .Select(group => new TopCategoryDto
                    {
                        CategoryName =
                            group.Key.Name,

                        Color =
                            group.Key.Color,

                        UsageCount =
                            group.Count()
                    })
                    .OrderByDescending(category =>
                        category.UsageCount)
                    .Take(5)
                    .ToListAsync();

            var totalCategoryUsage =
                await _context.UserMessageCategories
                    .CountAsync(item =>
                        item.Category.IsActive);

            foreach (var category in categoryStatistics)
            {
                category.Percentage =
                    totalCategoryUsage == 0
                        ? 0
                        : Math.Round(
                            category.UsageCount * 100.0 /
                            totalCategoryUsage,
                            1);
            }

            model.TopCategories = categoryStatistics;

            return View(model);
        }
    }
}