using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.AdminCategoryDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public AdminCategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .OrderBy(category => category.Name)
                .Select(category => new ResultCategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    Color = category.Color,
                    Icon = category.Icon,
                    IsActive = category.IsActive,

                    UsageCount = category.UserMessageCategories.Count
                })
                .ToListAsync();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateCategoryDto
            {
                Icon = "work",
                Color = "#004ac6",
                IsActive = true
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createCategoryDto);
            }

            var normalizedName = createCategoryDto.Name.Trim();

            var categoryExists = await _context.Categories
                .AnyAsync(category =>
                    category.Name.ToLower() ==
                    normalizedName.ToLower());

            if (categoryExists)
            {
                ModelState.AddModelError(
                    nameof(createCategoryDto.Name),
                    "Bu kategori zaten mevcut.");

                return View(createCategoryDto);
            }

            var category = new Category
            {
                Name = normalizedName,

                Description =
                    createCategoryDto.Description?.Trim()
                    ?? string.Empty,

                Color = createCategoryDto.Color,

                Icon = createCategoryDto.Icon.Trim(),

                IsActive = createCategoryDto.IsActive
            };

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Kategori başarıyla oluşturuldu.";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(category =>
                    category.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            var model = new UpdateCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Color = category.Color,
                Icon = category.Icon,
                IsActive = category.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateCategoryDto);
            }

            var category = await _context.Categories
                .FindAsync(updateCategoryDto.Id);

            if (category == null)
            {
                return NotFound();
            }

            var normalizedName =
                updateCategoryDto.Name.Trim();

            var duplicateExists = await _context.Categories
                .AnyAsync(item =>
                    item.Id != updateCategoryDto.Id &&
                    item.Name.ToLower() ==
                    normalizedName.ToLower());

            if (duplicateExists)
            {
                ModelState.AddModelError(
                    nameof(updateCategoryDto.Name),
                    "Bu isimde başka bir kategori zaten bulunuyor.");

                return View(updateCategoryDto);
            }

            category.Name = normalizedName;

            category.Description =
                updateCategoryDto.Description?.Trim()
                ?? string.Empty;

            category.Color =
                string.IsNullOrWhiteSpace(updateCategoryDto.Color)
                    ? "#004ac6"
                    : updateCategoryDto.Color;

            category.Icon =
                string.IsNullOrWhiteSpace(updateCategoryDto.Icon)
                    ? "label"
                    : updateCategoryDto.Icon.Trim();

            category.IsActive =
                updateCategoryDto.IsActive;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Kategori başarıyla güncellendi.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            category.IsActive = !category.IsActive;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = category.IsActive
                ? "Kategori aktifleştirildi."
                : "Kategori pasifleştirildi.";

            return RedirectToAction(nameof(Index));
        }
    }
}
