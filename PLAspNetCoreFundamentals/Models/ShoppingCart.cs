
using Microsoft.EntityFrameworkCore;

namespace PLAspNetCoreFundamentals.Models
{
    public class ShoppingCart : IShoppingCart
    {
        public readonly PieShopDbContext _pieShopDbContext;

        public string? shoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = default!;

        public ShoppingCart(PieShopDbContext pieShopDbContext)
        {
            _pieShopDbContext = pieShopDbContext;
        }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;

            PieShopDbContext context = services.GetService<PieShopDbContext>() ?? throw new Exception("Error Initializing");

            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();

            session?.SetString("CartId", cartId);

            return new ShoppingCart(context) { shoppingCartId = cartId };
        }
        public void AddToCart(Pie pie)
        {
            var shoppingCartItem = _pieShopDbContext.ShoppingCartItems.
                SingleOrDefault(s=>s.Pie.PieId == pie.PieId && s.ShoppingCartId == shoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = shoppingCartId,
                    Pie = pie,
                    Amount = 1
                };

                _pieShopDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _pieShopDbContext.SaveChanges();
        }

        public void ClearCart()
        {
            var cartItems = _pieShopDbContext
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == shoppingCartId);

            _pieShopDbContext.ShoppingCartItems.RemoveRange(cartItems);

            _pieShopDbContext.SaveChanges();
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??=
                       _pieShopDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == shoppingCartId)
                           .Include(s => s.Pie)
                           .ToList();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _pieShopDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == shoppingCartId)
                .Select(c => c.Pie.Price * c.Amount).Sum();
            return total;
        }

        public int RemoveFromCart(Pie pie)
        {
            var shoppingCartItem =
                    _pieShopDbContext.ShoppingCartItems.SingleOrDefault(
                        s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == shoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _pieShopDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            _pieShopDbContext.SaveChanges();

            return localAmount;
        }
    }
}
