using EntertainmentGuild.Data;
using EntertainmentGuild.DTO;
using EntertainmentGuild.Models;
using EntertainmentGuild.SessionHelper;
using EntertainmentGuild.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace EntertainmentGuild.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;
        const string SessionName = "_Name";
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            var obj = (from pro in _context.Products
                       join ger in _context.Genres on
                       pro.Genre equals ger.genreID
                       join stok in _context.Stocktakes
                       on pro.ID equals stok.ProductId
                       select new ShopViewModel
                       {
                           description = pro.Description,
                           GernreName = ger.Name,
                           name = pro.Name,
                           price = (double)stok.Price,
                           proid = pro.ID,
                           Quantity = (int)stok.Quantity,

                       }).ToList().Take(10);
            return View(obj);
        }
        public IActionResult Shop(string? productname)
        {
           ViewBag.Name = HttpContext.Session.GetString(SessionName);

            if (productname!=null)
            {
                var obj = (from pro in _context.Products
                           join ger in _context.Genres on
                           pro.Genre equals ger.genreID
                           join stok in _context.Stocktakes
                           on pro.ID equals stok.ProductId
                           where pro.Name==productname
                           select new ShopViewModel
                           {
                               description = pro.Description,
                               GernreName = ger.Name,
                               name = pro.Name,
                               price = (double)stok.Price,
                               proid = pro.ID,
                               Quantity = (int)stok.Quantity,

                           }).ToList().Take(50);
                return View(obj);
            }
            else
            {
                var obj = (from pro in _context.Products
                           join ger in _context.Genres on
                           pro.Genre equals ger.genreID
                           join stok in _context.Stocktakes
                           on pro.ID equals stok.ProductId
                           select new ShopViewModel
                           {
                               description = pro.Description,
                               GernreName = ger.Name,
                               name = pro.Name,
                               price = (double)stok.Price,
                               proid = pro.ID,
                               Quantity = (int)stok.Quantity,

                           }).ToList().Take(50);
                return View(obj);
            }
           
        }
        public IActionResult ProductDetail(int?id)
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            var obj = (from pro in _context.Products
                       join ger in _context.Genres on
                       pro.Genre equals ger.genreID
                       join stok in _context.Stocktakes
                       on pro.ID equals stok.ProductId
                       where pro.ID == id
                       select new ShopViewModel
                       {
                           description = pro.Description,
                           GernreName = ger.Name,
                           name = pro.Name,
                           price = (double)stok.Price,
                           proid = pro.ID,
                           Quantity = (int)stok.Quantity,

                       }).FirstOrDefault();

            return View(obj);
        }
        public IActionResult Buy(int id)
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);

            var book = _context.Products.Find(id);

            List<ProductItem> cart = new List<ProductItem>();

            cart = HttpContext.Session.Get<List<ProductItem>>("cart");
            if (cart == null)
            {
                cart = new List<ProductItem>();
                cart.Add(new ProductItem { Product = book, Quantity = 1 });
            }
            else
            {
                int index = cart.FindIndex(k => k.Product.ID == id);

                if (index != -1) //if item already in the cart
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new ProductItem { Product = book, Quantity = 1 });
                }
            }
            for (int i = 0; i < cart.Count; i++)
            {
                var b = _context.Products.Single(x => x.ID == cart[i].Product.ID);
            }
            HttpContext.Session.Set<List<ProductItem>>("cart", cart);
            return RedirectToAction("Cart");
        }
        public IActionResult Add(int id)
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            var cart = HttpContext.Session.Get<List<ProductItem>>("cart");
            var index = cart.FindIndex(m => m.Product.ID == id);
            cart[index].Quantity++;
            HttpContext.Session.Set<List<ProductItem>>("cart", cart);
            return RedirectToAction("Cart");
        }
        public IActionResult minus(int id)
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            var cart = HttpContext.Session.Get<List<ProductItem>>("cart");
            var index = cart.FindIndex(m => m.Product.ID == id);

            if (cart[index].Quantity == 1)
            {
                cart.RemoveAt(index);
            }
            else
            {
                cart[index].Quantity--;
            }

            HttpContext.Session.Set<List<ProductItem>>("cart", cart);
            return RedirectToAction("Cart");
        }
        public IActionResult Remove(int id)
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            _context.Products.Find(id);
            var cart = HttpContext.Session.Get<List<ProductItem>>("cart");
            var index = cart.FindIndex(m => m.Product.ID == id);
            cart.RemoveAt(index);
            HttpContext.Session.Set<List<ProductItem>>("cart", cart);
            return RedirectToAction("Cart");
        }
        public IActionResult Cart()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            var cart = HttpContext.Session.Get<List<ProductItem>>("cart");
            int cartCount = cart?.Sum(s => s.Quantity) ?? 0;
            ViewBag.CartCount = cartCount != 0 ? cartCount : (int?)null;

            if(cart!=null)
            { 
                var productPrices = new Dictionary<int, decimal>();
                foreach (var item in cart)
                {
                    int productId = item.Product.ID;
                    var price = _context.Stocktakes
                        .Where(x => x.ProductId == productId)
                        .Select(x => x.Price)
                        .FirstOrDefault();

                    productPrices[productId] = (decimal)price;
                }
                ViewBag.ProductPrices = productPrices;

            }
            if (cart == null)
            {
                cart = new List<ProductItem>();
            }

            return View(cart);
        }
        public IActionResult ProceedToCheckout(decimal productId)
        {
            TempData["StoreTotal"] = productId.ToString();
            return RedirectToAction("CheckOut");
        }
        public IActionResult CheckOut()
        {
            decimal totalPrice = decimal.Parse(TempData["StoreTotal"].ToString());
            ViewBag.Price = totalPrice;

            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            string username = ViewBag.Name;
            if (username != null)
            {
                var obj = (from patron in _context.Patrons
                           join to in _context.TOes on
                           patron.UserID equals to.PatronId
                           where patron.Name == username
                           select new UserDTO
                           {
                               Email = patron.Email,
                               CVV=(int)to.CVV,
                               ExpiryDate=to.Expiry,
                               CardNumber=to.CardNumber,
                               CardOwner=to.CardOwner,
                           }).FirstOrDefault();

                if(obj.CardNumber == null)
                {
                    ViewBag.CardNumber = "NotFound";
                }
                return View(obj);
            }
            return View();
        }
        [HttpPost]
        public IActionResult CheckOut(UserDTO userDTO)
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            string username = ViewBag.Name;
            var obj = _context.Patrons.Where(x => x.Name == username).FirstOrDefault();
            var usert = _context.TOes.Where(x => x.PatronId == obj.UserID).Select(x => x.customerID).FirstOrDefault();
            Order order = new Order
            {
                customer = usert,
                StreetAddress = userDTO.StreetAdress,
                PostCode = Convert.ToInt32(userDTO.PostalCode),
                State = userDTO.State,
                Suburb = userDTO.Suburb,
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
            //for orderdeatil
            var cart = HttpContext.Session.Get<List<ProductItem>>("cart");

            for (int i = 0; i < cart.Count; i++)
            {
                ProductInOrders od = new ProductInOrders
                {
                    Quantity = cart[i].Quantity,
                    OrderId = order.OrderID,
                    produktId = cart[i].Product.ID,
                };
                _context.ProductsInOrders.Add(od);
            }

            _context.SaveChanges();
            HttpContext.Session.Remove("cart");
            return RedirectToAction("Index");
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult UserLogin()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            return View();
        }
        [HttpPost]
        public IActionResult UserLogin(UserDTO userDTO)
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            if (userDTO.Email != null && userDTO.Password != null)
            {
                var obj = _context.Patrons.Where(x => x.Email == userDTO.Email).FirstOrDefault();

                if (obj != null)
                {
                    string recomputedHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password, obj.Salt);
                    if (recomputedHash == obj.HashPW)
                    {
                        HttpContext.Session.SetString(SessionName, obj.Name);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Login attempt");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login attempt");
                    return View();
                }
            }

            return View();
        }
        public IActionResult UserForgetPassword()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            return View();
        }
        [HttpPost]
        public IActionResult UserForgetPassword(UserDTO userDTO)
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            if (userDTO.Email != null)
            {
                var obj = _context.Patrons.Where(x => x.Email == userDTO.Email).FirstOrDefault();
                if (obj != null)
                {
                    string newSalt = BCrypt.Net.BCrypt.GenerateSalt();
                    string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password, newSalt);
                    obj.Salt = newSalt;
                    obj.HashPW = newHashedPassword;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User name not found");

                    return View();
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult AdminLogin()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            return View();
        }
        [HttpPost]
        public IActionResult AdminLogin(UserDTO userDTO)
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            if (userDTO.UserName != null && userDTO.Password != null)
            {
                var obj = _context.Users.Where(x => x.UserName == userDTO.UserName).FirstOrDefault();
                
                if (obj != null)
                {
                    string recomputedHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password, obj.Salt);
                    if (recomputedHash == obj.HashPW)
                    {
                        HttpContext.Session.SetString(SessionName, obj.UserName);                     
                        return RedirectToAction("Index", "Admin");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login attempt");
                    return View();
                }
            }

            return View();
            
        }
        public IActionResult Register()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserDTO userDTO)
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            var obj = _context.Patrons.Where(x => x.Email == userDTO.Email).FirstOrDefault();
            if (obj == null)
            {
                string newSalt = BCrypt.Net.BCrypt.GenerateSalt();
                string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password, newSalt);
                Patron patron = new Patron
                {
                    Email = userDTO.Email,
                    Name = userDTO.Name,
                    HashPW = newHashedPassword,
                    Salt = newSalt
                };
                _context.Patrons.Add(patron);
                _context.SaveChanges();
                TO tO = new TO
                {
                    PatronId=patron.UserID,
                    Email=patron.Email,
                    PhoneNumber = userDTO.PhoneNumber,
                    PostCode=Convert.ToInt32(userDTO.PostalCode),
                    State=userDTO.State,
                    Suburb=userDTO.Suburb,
                    StreetAddress=userDTO.StreetAdress,
                };
                _context.TOes.Add(tO);
                _context.SaveChanges();

                return RedirectToAction("UserLogin");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This Email is already taken");
                return View(userDTO);
            }


        }
        public IActionResult ForgetPassword()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            return View();
        }
        [HttpPost]
        public IActionResult ForgetPassword(UserDTO userDTO)
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            if (userDTO.UserName != null)
            {
                var obj = _context.Users.Where(x => x.UserName == userDTO.UserName).FirstOrDefault();
                if (obj != null)
                {                 
                    string newSalt = BCrypt.Net.BCrypt.GenerateSalt();
                    string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password, newSalt);
                    obj.Salt = newSalt;
                    obj.HashPW = newHashedPassword;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User name not found");

                    return View();
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Logout()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            HttpContext.Session.Remove(SessionName);
            return RedirectToAction("Index");
        }
        public IActionResult UserDashborad()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            return View();
        }
        public IActionResult Profile()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            string username = ViewBag.Name;
            if(username!=null)
            {
                var obj = (from patron in _context.Patrons
                           join to in _context.TOes on
                           patron.UserID equals to.PatronId
                           where patron.Name == username
                           select new UserDTO
                           {
                               Email=patron.Email,
                               Name=patron.Name,
                               PhoneNumber=to.PhoneNumber,
                               PostalCode=to.PostCode.ToString(),
                               StreetAdress=to.StreetAddress,
                               State=to.State,
                               Suburb=to.Suburb,                              
                           }).FirstOrDefault();
                return View(obj);

            }
            return View();
        }
        [HttpPost]
        public IActionResult Profile(UserDTO userDTO)
        {    
            if(userDTO.Email!=null)
            {
                var obj = _context.Patrons.Where(x => x.Email == userDTO.Email).FirstOrDefault();
                if (obj != null)
                {
                    obj.Name = userDTO.Name;
                    // Update other properties as needed

                    var usert = _context.TOes.Where(x => x.PatronId == obj.UserID).FirstOrDefault();
                    if (usert != null)
                    {
                        // Update TO information
                        usert.PhoneNumber = userDTO.PhoneNumber;
                        usert.PostCode = Convert.ToInt32(userDTO.PostalCode);
                        usert.StreetAddress = userDTO.StreetAdress;
                        usert.Suburb = userDTO.Suburb;
                        usert.State = userDTO.State;

                        // Save changes to the database
                        _context.SaveChanges();
                    }
                }
            }
            return RedirectToAction("UserDashborad");
        }

        public IActionResult AddCardInfo()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            string username = ViewBag.Name;
            if (username != null)
            {
                var obj = (from patron in _context.Patrons
                           join to in _context.TOes on
                           patron.UserID equals to.PatronId
                           where patron.Name == username
                           select new UserDTO
                           {                            
                               CardOwner = to.CardOwner,
                               ExpiryDate = to.Expiry,
                               CVV = (int)to.CVV,
                               CardNumber = to.CardNumber,                            
                           }).FirstOrDefault();
                return View(obj);
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddCardInfo(UserDTO userDTO)
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            string username = ViewBag.Name;
            var obj= _context.Patrons.Where(x=>x.Name==username).FirstOrDefault();

            if (obj != null)
            {
                var usert = _context.TOes.Where(x => x.PatronId == obj.UserID).FirstOrDefault();
                if (usert != null)
                {
                    // Update TO information
                    usert.CardNumber = userDTO.CardNumber;
                    usert.Expiry = userDTO.ExpiryDate;
                    usert.CVV = userDTO.CVV;
                    usert.CardOwner = userDTO.CardOwner;

                    // Save changes to the database
                    _context.SaveChanges();
                    return RedirectToAction("UserDashborad");
                }
                return View();
            }

            return View();
        }
        public IActionResult OrderHistory()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}