using FileStorageSystem.Data;
using FileStorageSystem.Model;
using FileStorageSystem.Security;
using Microsoft.AspNetCore.Mvc;

namespace FileStorageSystem.Controllers
{
    public class LoginController : Controller
    {

        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string userId, string Password)
        {
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(Password))
            {
                string decPassword = EncryptionHelper.doEncrypt(Password);
                var user = _context.Users.Where(x => x.UserName == userId && x.Passward == decPassword).FirstOrDefault();
                if (user != null)
                {
                    HttpContext.Session.SetString("userId", userId);
                    HttpContext.Session.SetString("fullName", user.FullName);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Invalid User Password";
                    return View();
                }
            }


            return View();
        }

        public IActionResult SignUp()
        {
            ViewBag.error = TempData["error"];
            ViewBag.success = TempData["success"];
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User u, string cpassword)
        {
            if (u.Passward != cpassword)
            {
                TempData["error"] = "Password and Confirm Password Not match";
                return RedirectToAction("SignUp");
            }

            string hasPassword = EncryptionHelper.doEncrypt(u.Passward);
            u.Passward = hasPassword;
            u.RoleId = 1;
            u.CreateDate = DateTime.Now;
            u.ActiveStatus = true;
            _context.Add(u);
            if (_context.SaveChanges() > 0)
            {
                TempData["success"] = "Register Successfully";
            }
            else
            {
                TempData["error"] = "Registeration failed ";
            }

            return RedirectToAction("SignUp");
        }

    }
}
