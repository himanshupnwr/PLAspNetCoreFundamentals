using Microsoft.AspNetCore.Mvc;

namespace PLAspNetCoreFundamentals.Controllers
{
    public class ContactController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
