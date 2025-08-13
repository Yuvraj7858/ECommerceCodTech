using Dukaan.Models;
using Dukaan.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dukaan.Extensions;
using Microsoft.AspNetCore.Http;
using Dukaan.Models; // CartItem model

public class CartController : Controller
{
    private readonly AppDbContext _context;
    private const string CartSessionKey = "CartSession";

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    // Add product to cart
    public async Task<IActionResult> AddToCart(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            return NotFound();

        List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();

        var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
        if (cartItem != null)
        {
            cartItem.Quantity++;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1
            });
        }

        HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);

        return RedirectToAction("ViewCart");
    }

    // View cart items
    public IActionResult ViewCart()
    {
        List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
        return View(cart);
    }

    // Remove product from cart
    public IActionResult RemoveFromCart(int productId)
    {
        List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();

        var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
        if (cartItem != null)
        {
            cart.Remove(cartItem);
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }
        return RedirectToAction("ViewCart");
    }

    // Update quantity
    [HttpPost]
    public IActionResult UpdateQuantity(int productId, int quantity)
    {
        List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();

        var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
        if (cartItem != null)
        {
            if (quantity > 0)
            {
                cartItem.Quantity = quantity;
            }
            else
            {
                cart.Remove(cartItem);
            }
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }

        return RedirectToAction("ViewCart");
    }
}
