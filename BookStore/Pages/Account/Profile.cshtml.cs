using BookStore.Data;
using BookStore.Model.AuthApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookStore.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProfileModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public AuthUser AppUser { get; set; }

        [BindProperty]
        public IFormFile? AvatarFile { get; set; }

        public string? CurrentAvatarPath { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var id = int.Parse(userId);
            AppUser = await _context.AuthUsers.FindAsync(id);

            if (AppUser == null)
                return NotFound();

            CurrentAvatarPath = AppUser.AvatarPath;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var id = int.Parse(userId);
            var userFromDb = await _context.AuthUsers.FindAsync(id);

            if (userFromDb == null)
                return NotFound();

            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var ext = Path.GetExtension(AvatarFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("AvatarFile", "Разрешены только изображения (jpg, png, gif, webp)");
                    return Page();
                }

                if (AvatarFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("AvatarFile", "Максимальный размер файла 5MB");
                    return Page();
                }

                if (!string.IsNullOrEmpty(userFromDb.AvatarPath))
                {
                    var oldPath = Path.Combine(_environment.WebRootPath, userFromDb.AvatarPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "avatars");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(AvatarFile.FileName)}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await AvatarFile.CopyToAsync(stream);
                }

                userFromDb.AvatarPath = $"/uploads/avatars/{fileName}";
                await _context.SaveChangesAsync();

                TempData["Success"] = "Аватар успешно обновлен!";
            }

            return RedirectToPage();
        }
    }

}
