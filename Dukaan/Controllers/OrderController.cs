using Dukaan.Extensions;
using Dukaan.Models;
using Dukaan.Data;
using Microsoft.AspNetCore.Mvc;

public class OrderController : Controller
{
    private readonly AppDbContext _context;
    private const string CartSessionKey = "CartSession";

    public OrderController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Checkout()
    {
        List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey);
        if (cart == null || cart.Count == 0)
        {
            return RedirectToAction("Index", "Products");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(string customerName, string address, string phone)
    {
        List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey);
        if (cart == null || cart.Count == 0)
        {
            ModelState.AddModelError("", "Your cart is empty");
            return View();
        }

        var order = new Order
        {
            CustomerName = customerName,
            Address = address,
            Phone = phone,
            OrderDate = DateTime.Now,
            OrderItems = cart.Select(c => new OrderItem
            {
                ProductId = c.ProductId,
                Quantity = c.Quantity,
                Price = c.Price
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        HttpContext.Session.Remove(CartSessionKey); // Clear cart after order

        return RedirectToAction("OrderSuccess");
    }

    public IActionResult OrderSuccess()
    {
        return View();
    }
}
