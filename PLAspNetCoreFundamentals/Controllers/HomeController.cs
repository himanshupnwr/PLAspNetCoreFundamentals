using Microsoft.AspNetCore.Mvc;
using PLAspNetCoreFundamentals.Models;
using PLAspNetCoreFundamentals.ViewModel;
using System.Diagnostics;

namespace PLAspNetCoreFundamentals.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPieRepository _pieRepository;

        public HomeController(IPieRepository pieRepository)
        {
            _pieRepository = pieRepository;
        }

        public ViewResult Index()
        {
            var piesOfTheWeek = _pieRepository.PiesOfTheWeek;

            var homeViewModel = new HomeViewModel(piesOfTheWeek);

            return View(homeViewModel);
        }
    }
}
