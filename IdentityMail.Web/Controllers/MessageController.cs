using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.MessageReportDtos;
using IdentityMail.Web.DTOs.UserMessageDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Controllers
{
    [Authorize]
    public class MessageController(UserManager<AppUser> _userManager,
                                           AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.fullName = $"{user.FirstName} {user.LastName}";

            var messages = await _context.UserMessages
                .AsNoTracking()
                .Include(x => x.Sender)
                .Where(x =>
                    x.ReceiverId == user.Id &&
                    !x.IsTrashedByReceiver &&
                    !x.IsPermanentlyDeletedByReceiver)
                .OrderByDescending(x => x.SendTime)
                .ToListAsync();

            return View(messages);
        }
        [HttpGet]
        public async Task<IActionResult> SendMail()
        {
            await LoadCategoriesAsync();

            var model = new SendMailDto();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMail(
    SendMailDto sendMailDto)
        {
            var sender = await _userManager.GetUserAsync(User);

            if (sender == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(sendMailDto.CategoryId);

                return View(sendMailDto);
            }

            var receiver = await _userManager.FindByEmailAsync(
                sendMailDto.ReceiverMail);

            if (receiver == null)
            {
                ModelState.AddModelError(
                    nameof(sendMailDto.ReceiverMail),
                    "Girdiğiniz e-posta adresine kayıtlı kullanıcı bulunamadı.");

                await LoadCategoriesAsync(sendMailDto.CategoryId);

                return View(sendMailDto);
            }

            if (receiver.Id == sender.Id)
            {
                ModelState.AddModelError(
                    nameof(sendMailDto.ReceiverMail),
                    "Kendinize mesaj gönderemezsiniz.");

                await LoadCategoriesAsync(sendMailDto.CategoryId);

                return View(sendMailDto);
            }

            if (sendMailDto.CategoryId.HasValue)
            {
                var categoryExists = await _context.Categories
                    .AsNoTracking()
                    .AnyAsync(category =>
                        category.Id == sendMailDto.CategoryId.Value &&
                        category.IsActive);

                if (!categoryExists)
                {
                    ModelState.AddModelError(
                        nameof(sendMailDto.CategoryId),
                        "Seçilen kategori bulunamadı veya pasif durumdadır.");

                    await LoadCategoriesAsync(sendMailDto.CategoryId);

                    return View(sendMailDto);
                }
            }

            var newMessage = new UserMessage
            {
                SenderId = sender.Id,
                ReceiverId = receiver.Id,

                Subject = sendMailDto.Subject.Trim(),
                Body = sendMailDto.Body.Trim(),

                SendTime = DateTime.Now,

                IsRead = false,
                IsImportant = false,

                IsTrashedBySender = false,
                IsTrashedByReceiver = false,

                IsPermanentlyDeletedBySender = false,
                IsPermanentlyDeletedByReceiver = false
            };

            _context.UserMessages.Add(newMessage);

            await _context.SaveChangesAsync();

            if (sendMailDto.CategoryId.HasValue)
            {
                var messageCategory = new UserMessageCategory
                {
                    UserId = sender.Id,
                    UserMessageId = newMessage.Id,
                    CategoryId = sendMailDto.CategoryId.Value
                };

                _context.UserMessageCategories.Add(messageCategory);
            }

            if (sendMailDto.DraftId.HasValue)
            {
                var draft = await _context.MailDrafts
                    .FirstOrDefaultAsync(draft =>
                        draft.Id == sendMailDto.DraftId.Value &&
                        draft.OwnerId == sender.Id);

                if (draft != null)
                {
                    _context.MailDrafts.Remove(draft);
                }
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Mesajınız başarıyla gönderildi.";

            return RedirectToAction(nameof(Sent));
        }
        public async Task<IActionResult> MailDetail(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var message = await _context.UserMessages
                .Include(x => x.Sender)
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.ReceiverId == user.Id);

            if (message == null)
            {
                return NotFound();
            }

            if (!message.IsRead)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }

            ViewBag.IsReported = await _context.MessageReports
                .AnyAsync(report =>
                    report.MessageId == message.Id &&
                    report.ReportedByUserId == user.Id);

            return View(message);
        }

        public async Task<IActionResult> Sent()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var sentMessages = await _context.UserMessages
                .AsNoTracking()
                .Include(x => x.Receiver)
                .Where(x =>
                    x.SenderId == user.Id &&
                    !x.IsTrashedBySender &&
                    !x.IsPermanentlyDeletedBySender)
                .OrderByDescending(x => x.SendTime)
                .ToListAsync();

            return View(sentMessages);
        }
        public async Task<IActionResult> SentMessageDetail(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var message = await _context.UserMessages
                .AsNoTracking()
                .Include(x => x.Receiver)
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.SenderId == user.Id);

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveToTrash(
    int id,
    string? returnAction)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var message = await _context.UserMessages
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    (
                        x.SenderId == user.Id ||
                        x.ReceiverId == user.Id
                    ));

            if (message == null)
            {
                return NotFound();
            }

            if (message.ReceiverId == user.Id)
            {
                message.IsTrashedByReceiver = true;
            }
            else if (message.SenderId == user.Id)
            {
                message.IsTrashedBySender = true;
            }

            await _context.SaveChangesAsync();

            if (returnAction == "Important")
            {
                return RedirectToAction("Important");
            }

            if (returnAction == "Sent")
            {
                return RedirectToAction("Sent");
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Trash()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var trashMessages = await _context.UserMessages
                .AsNoTracking()
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Where(x =>
                    (
                        x.ReceiverId == user.Id &&
                        x.IsTrashedByReceiver &&
                        !x.IsPermanentlyDeletedByReceiver
                    )
                    ||
                    (
                        x.SenderId == user.Id &&
                        x.IsTrashedBySender &&
                        !x.IsPermanentlyDeletedBySender
                    ))
                .OrderByDescending(x => x.SendTime)
                .ToListAsync();

            ViewBag.CurrentUserId = user.Id;

            return View(trashMessages);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreMessage(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var message = await _context.UserMessages
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    (
                        x.SenderId == user.Id ||
                        x.ReceiverId == user.Id
                    ));

            if (message == null)
            {
                return NotFound();
            }

            if (message.ReceiverId == user.Id)
            {
                message.IsTrashedByReceiver = false;
            }

            if (message.SenderId == user.Id)
            {
                message.IsTrashedBySender = false;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Trash");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PermanentlyDelete(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var message = await _context.UserMessages
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    (
                        x.SenderId == user.Id ||
                        x.ReceiverId == user.Id
                    ));

            if (message == null)
            {
                return NotFound();
            }

            if (message.ReceiverId == user.Id)
            {
                if (!message.IsTrashedByReceiver)
                {
                    return BadRequest();
                }

                message.IsPermanentlyDeletedByReceiver = true;
            }

            if (message.SenderId == user.Id)
            {
                if (!message.IsTrashedBySender)
                {
                    return BadRequest();
                }

                message.IsPermanentlyDeletedBySender = true;
            }

            if (message.IsPermanentlyDeletedBySender &&
                message.IsPermanentlyDeletedByReceiver)
            {
                _context.UserMessages.Remove(message);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Trash");
        }
        [HttpGet]
        public async Task<IActionResult> Important()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var importantMessages = await _context.UserMessages
                .AsNoTracking()
                .Include(x => x.Sender)
                .Where(x =>
                    x.ReceiverId == user.Id &&
                    x.IsImportant &&
                    !x.IsTrashedByReceiver &&
                    !x.IsPermanentlyDeletedByReceiver)
                .OrderByDescending(x => x.SendTime)
                .ToListAsync();

            return View(importantMessages);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeImportantStatus(int id, string? returnAction)
        {
            var user = await _userManager.FindByNameAsync(
                User.Identity.Name
            );

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var message = await _context.UserMessages
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.ReceiverId == user.Id &&
                    !x.IsTrashedByReceiver &&
                    !x.IsPermanentlyDeletedByReceiver);

            if (message == null)
            {
                return NotFound();
            }

            message.IsImportant = !message.IsImportant;

            await _context.SaveChangesAsync();

            if (returnAction == "Important")
            {
                return RedirectToAction("Important");
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDraft(
    SaveDraftDto saveDraftDto)
        {
            var user = await _userManager.FindByNameAsync(
                User.Identity.Name
            );

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (saveDraftDto.DraftId.HasValue)
            {
                var draft = await _context.MailDrafts
                    .FirstOrDefaultAsync(x =>
                        x.Id == saveDraftDto.DraftId.Value &&
                        x.OwnerId == user.Id);

                if (draft == null)
                {
                    return NotFound();
                }

                draft.ReceiverMail =
                    saveDraftDto.ReceiverMail;

                draft.Subject =
                    saveDraftDto.Subject;

                draft.Body =
                    saveDraftDto.Body;

                draft.UpdatedDate = DateTime.Now;
            }
            else
            {
                var draft = new MailDraft
                {
                    OwnerId = user.Id,
                    ReceiverMail = saveDraftDto.ReceiverMail,
                    Subject = saveDraftDto.Subject,
                    Body = saveDraftDto.Body,
                    UpdatedDate = DateTime.Now
                };

                await _context.MailDrafts.AddAsync(draft);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Taslak başarıyla kaydedildi.";

            return RedirectToAction("Drafts");
        }
        [HttpGet]
        public async Task<IActionResult> Drafts()
        {
            var user = await _userManager.FindByNameAsync(
                User.Identity.Name
            );

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var drafts = await _context.MailDrafts
                .AsNoTracking()
                .Where(x => x.OwnerId == user.Id)
                .OrderByDescending(x => x.UpdatedDate)
                .ToListAsync();

            return View(drafts);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDraft(int id)
        {
            var user = await _userManager.FindByNameAsync(
                User.Identity.Name
            );

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var draft = await _context.MailDrafts
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.OwnerId == user.Id);

            if (draft == null)
            {
                return NotFound();
            }

            _context.MailDrafts.Remove(draft);

            await _context.SaveChangesAsync();

            return RedirectToAction("Drafts");
        }
        [HttpGet]
        public async Task<IActionResult> EditDraft(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var draft = await _context.MailDrafts
                .AsNoTracking()
                .FirstOrDefaultAsync(draft =>
                    draft.Id == id &&
                    draft.OwnerId == user.Id);

            if (draft == null)
            {
                return NotFound();
            }

            var model = new SendMailDto
            {
                DraftId = draft.Id,
                ReceiverMail = draft.ReceiverMail,
                Subject = draft.Subject,
                Body = draft.Body
            };

            await LoadCategoriesAsync(model.CategoryId);

            return View("SendMail", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignCategory(int messageId, int categoryId)
        {
            var currentUser = await _userManager
                .GetUserAsync(User);

            if (currentUser == null)
            {
                return Challenge();
            }

            var message = await _context.UserMessages
                .AsNoTracking()
                .FirstOrDefaultAsync(message =>
                    message.Id == messageId);

            if (message == null)
            {
                return NotFound();
            }

            var userIsParticipant =
                message.SenderId == currentUser.Id ||
                message.ReceiverId == currentUser.Id;

            if (!userIsParticipant)
            {
                return Forbid();
            }

            var categoryIsActive = await _context.Categories
                .AnyAsync(category =>
                    category.Id == categoryId &&
                    category.IsActive);

            if (!categoryIsActive)
            {
                TempData["ErrorMessage"] =
                    "Seçilen kategori bulunamadı veya pasif.";

                return RedirectToAction(nameof(Index));
            }

            var alreadyAssigned =
                await _context.UserMessageCategories
                    .AnyAsync(item =>
                        item.UserId == currentUser.Id &&
                        item.UserMessageId == messageId &&
                        item.CategoryId == categoryId);

            if (!alreadyAssigned)
            {
                var messageCategory = new UserMessageCategory
                {
                    UserId = currentUser.Id,
                    UserMessageId = messageId,
                    CategoryId = categoryId
                };

                _context.UserMessageCategories.Add(messageCategory);

                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] =
    alreadyAssigned
        ? "Mesaj bu kategoriye zaten eklenmiş."
        : "Mesaj kategoriye eklendi.";

            if (message.ReceiverId == currentUser.Id)
            {
                return RedirectToAction(
                    nameof(MailDetail),
                    new { id = messageId });
            }

            return RedirectToAction(
                nameof(SentMessageDetail),
                new { id = messageId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCategory(int messageId, int categoryId)
        {
            var currentUser = await _userManager
                .GetUserAsync(User);

            if (currentUser == null)
            {
                return Challenge();
            }

            var messageCategory =
                await _context.UserMessageCategories
                    .FirstOrDefaultAsync(item =>
                        item.UserId == currentUser.Id &&
                        item.UserMessageId == messageId &&
                        item.CategoryId == categoryId);

            if (messageCategory == null)
            {
                return NotFound();
            }

            _context.UserMessageCategories.Remove(messageCategory);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Mesaj kategoriden çıkarıldı.";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Report(int id)
        {
            var currentUser =
                await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Challenge();
            }

            var messageExists = await _context.UserMessages
                .AsNoTracking()
                .AnyAsync(message =>
                    message.Id == id &&
                    message.ReceiverId == currentUser.Id);

            if (!messageExists)
            {
                return NotFound();
            }

            var alreadyReported = await _context.MessageReports
                .AnyAsync(report =>
                    report.MessageId == id &&
                    report.ReportedByUserId == currentUser.Id);

            if (alreadyReported)
            {
                TempData["ErrorMessage"] =
                    "Bu mesajı daha önce şikâyet ettiniz.";

                return RedirectToAction(
                    "MailDetail",
                    new { id });
            }

            return View(new CreateMessageReportDto
            {
                MessageId = id
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(
    CreateMessageReportDto model)
        {
            var currentUser =
                await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Challenge();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var messageExists = await _context.UserMessages
                .AsNoTracking()
                .AnyAsync(message =>
                    message.Id == model.MessageId &&
                    message.ReceiverId == currentUser.Id);

            if (!messageExists)
            {
                return NotFound();
            }

            var alreadyReported = await _context.MessageReports
                .AnyAsync(report =>
                    report.MessageId == model.MessageId &&
                    report.ReportedByUserId == currentUser.Id);

            if (alreadyReported)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Bu mesajı daha önce şikâyet ettiniz.");

                return View(model);
            }

            var report = new MessageReport
            {
                MessageId = model.MessageId,
                ReportedByUserId = currentUser.Id,
                Reason = model.Reason.Trim(),
                Description = model.Description?.Trim(),
                ReportDate = DateTime.Now,
                IsResolved = false
            };

            _context.MessageReports.Add(report);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Şikâyetiniz yöneticiye iletildi.";

            return RedirectToAction(
                "MailDetail",
                new { id = model.MessageId });
        }
        private async Task LoadCategoriesAsync(
    int? selectedCategoryId = null)
        {
            ViewBag.Categories = await _context.Categories
                .AsNoTracking()
                .Where(category => category.IsActive)
                .OrderBy(category => category.Name)
                .Select(category => new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name,
                    Selected = category.Id == selectedCategoryId
                })
                .ToListAsync();
        }
    }
}
