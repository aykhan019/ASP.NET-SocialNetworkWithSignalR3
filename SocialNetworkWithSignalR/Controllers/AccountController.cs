using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkWithSignalR.Entities;
using SocialNetworkWithSignalR.Helpers;
using SocialNetworkWithSignalR.Models;
using System.Data;

namespace SocialNetworkWithSignalR.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<CustomIdentityUser> _userManager;
        private RoleManager<CustomIdentityRole> _roleManager;
        private SignInManager<CustomIdentityUser> _signInManager;
        private readonly IWebHostEnvironment _webhost;
        private IHttpContextAccessor httpContextAccessor;
        private CustomIdentityDbContext _dbContext;
        public AccountController(UserManager<CustomIdentityUser> userManager,
            RoleManager<CustomIdentityRole> roleManager,
            SignInManager<CustomIdentityUser> signInManager,
            IWebHostEnvironment webhost,
            CustomIdentityDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _webhost = webhost;
            _dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        // GET: AccountController
        public ActionResult Register()
        {
            return View();
        }

    

        [Authorize(Roles = "Admin")]
        public ActionResult RegisterEditor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var helper = new ImageHelper(_webhost);
                model.ImageUrl = await helper.SaveFile(model.File);
                CustomIdentityUser user = new CustomIdentityUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    ImageUrl = model.ImageUrl,
                };
                IdentityResult result = _userManager.CreateAsync(user, model.Password).Result;

                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("Admin").Result)
                    {
                        CustomIdentityRole role = new CustomIdentityRole
                        {
                            Name = "Admin"
                        };

                        IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError("", "We can not add the role");
                            return View(model);
                        }
                    }

                    _userManager.AddToRoleAsync(user, "Admin").Wait();
                    return RedirectToAction("Login", "Account");
                }
            }
            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false).Result;
                if (result.Succeeded)
                {
                    var user = _dbContext.Users.SingleOrDefault(u => u.UserName == model.Username);
                    if (user != null)
                    {
                    user.ConnectTime = DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString();
                    _dbContext.Users.Update(user);
                    await _dbContext.SaveChangesAsync();
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid Login");
            }
            return View(model);
        }


        public async Task<IActionResult> LogOut()
        {
            _signInManager.SignOutAsync().Wait();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            user.DisConnectTime = DateTime.Now;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
