using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using NetTask8.DataAccess;
using NetTask8.Models;
using NetTask8.Utility;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NetTask8.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NetTask8.Controllers
{
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        public RequestController(ApplicationDbContext db, IWebHostEnvironment env, UserManager<ApplicationUser> usmngr)
        {
            _db = db;
            _env = env;
            _userManager = usmngr;
        }

        public IActionResult Index()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var requests = _db.Requests.Include(i => i.Approval1User).Include(j => j.Approval2User).Include(k => k.Category).Where(u => u.CreatorId == userId).OrderBy(x => x.Id).ToList();
            return View(requests);
        }

        public IActionResult ListAllRequsts()
        {
            var requests = _db.Requests.Include(i => i.Approval1User).Include(j => j.Approval2User).Include(k => k.Category).Include(l => l.User).OrderBy(x => x.Id).ToList();
            return View(requests);
        }

        public async Task <IActionResult> ListRequsts()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);

            var requests = _db.Requests.Include(u => u.User).Include(n => n.Category).Where(i => i.Status == StaticDetails.Initiated);
            if (user?.Email == "User2@site.com")
            {
                requests = requests.Where(i => i.Approval1 == null || i.Approval1 == false);
            }
            else if (user?.Email == "User3@site.com")
            {
                requests = requests.Where(i => (i.Approval1 != null && i.Approval1 == true) && (i.Approval2 == null || i.Approval2 == false));
            }

            requests = requests.OrderBy(x => x.Id);

            return View(await requests.ToListAsync());
        }

        public IActionResult Details(int? id)
        {
            if (id is null || id == 0) return NotFound();
            var request = _db.Requests.Include(i => i.Category).FirstOrDefault(x => x.Id == id);
            if (request is null) return NotFound();
            return View(request);
        }

        public IActionResult Create()
        {
            var vm = new RequestCategoryViewModel();
            var list = _db.Categories.OrderBy(x => x.Name).Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();
            vm.Categories = list;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestCategoryViewModel obj, IFormFile? file)
        {
            if (file == null || file.Length == 0)
                ModelState.AddModelError("file", "Please upload a file");

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            string fileName = Guid.NewGuid() + Path.GetExtension(file!.FileName);
            string fullPath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var request = new Request
            {
                FileNumber = obj.FileNumber,
                Subject = obj.Subject,
                CategoryId = obj.CategoryId!.Value,
                FilePath = "/uploads/" + fileName,
                Date = DateTime.UtcNow,
                CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!
            };

            _db.Requests.Add(request);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Download(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return BadRequest("File path is required.");
            }

            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

            string fullPath = Path.Combine(uploadsFolder, Path.GetFileName(filePath));

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound("File not found.");
            }

            return PhysicalFile(fullPath, "application/octet-stream", Path.GetFileName(fullPath));
        }

        public IActionResult ChangeStatus(int? id)
        {
            if (id is null || id == 0) return NotFound();
            var request = _db.Requests.FirstOrDefault(x => x.Id == id);
            if (request is null) return NotFound();
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            var request = _db.Requests.FirstOrDefault(x => x.Id == id);
            if (request is null) return NotFound();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);
            

            if (user.Email == "User2@site.com")
            {
                request.Approval1UserId = userId;
                if (status == StaticDetails.Approved)
                {
                    request.Approval1 = true;
                }
                else
                {
                    request.Approval1 = false;
                    request.Status = StaticDetails.Rejected;
                }
            }

            if (user.Email == "User3@site.com")
            {
                request.Approval2UserId = userId;
                if (status == StaticDetails.Approved)
                {
                    request.Approval2 = true;
                    request.Status = StaticDetails.Approved;
                }
                else
                {
                    request.Approval2 = false;
                    request.Status = StaticDetails.Rejected;
                }
            }

            _db.SaveChanges();
            return RedirectToAction("ListRequsts");
        }
    }
}
