using EntertainmentGuild.Data;
using EntertainmentGuild.DTO;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Mvc;

namespace EntertainmentGuild.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        const string SessionName = "_Name";
        public AdminController(ApplicationDbContext context)
        {
            _context = context; 
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UserList()
        {
            var obj= (from Pat in _context.Patrons join to in _context.TOes on
                     Pat.UserID equals to.PatronId select new UserDTO
                     {
                         Email= Pat.Email,
                         Name= Pat.Name,
                         StreetAdress=to.StreetAddress,
                         PhoneNumber=to.PhoneNumber,    
                         PostalCode=to.PostCode.ToString(),
                         State=to.State,

                     }).ToList();
            return View(obj);
        }
        public IActionResult OrderList()
        {
            var obj = (from od in _context.Orders  
                       join to in _context.TOes on
                       od.customer equals to.customerID
                       select new UserDTO
                       {
                           PostalCode=od.PostCode.ToString(),
                           StreetAdress=od.StreetAddress,
                           State=od.State,
                           Suburb=od.Suburb,
                           Email=to.Email,
                           PhoneNumber=to.PhoneNumber

                       }).ToList();
            return View(obj);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionName);
            return RedirectToAction("Index","Home");
        }
    }
}
