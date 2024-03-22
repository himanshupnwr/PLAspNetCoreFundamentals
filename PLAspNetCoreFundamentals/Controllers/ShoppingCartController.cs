using Microsoft.AspNetCore.Mvc;
using PLAspNetCoreFundamentals.Models;
using PLAspNetCoreFundamentals.ViewModel;

namespace PLAspNetCoreFundamentals.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly IShoppingCart _shoppingCart;

        public ShoppingCartController(IPieRepository pieRepository, IShoppingCart shoppingCart)
        {
            _pieRepository = pieRepository;
            _shoppingCart = shoppingCart;

        }

        public ViewResult Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel(_shoppingCart, _shoppingCart.GetShoppingCartTotal());

            return View (shoppingCartViewModel);
        }

        public RedirectToActionResult AddToShoppingCart(int PieId)
        {
            var selectedPie = _pieRepository.GetPieById(PieId);

            if(selectedPie != null)
            {
                _shoppingCart.AddToCart(selectedPie);
            }

            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromShoppingCart(int PieId)
        {
            var selectedPie = _pieRepository.GetPieById(PieId);

            if(selectedPie != null)
            {
                _shoppingCart.RemoveFromCart(selectedPie);
            }
            return RedirectToAction("Index");
        }
    }
}
