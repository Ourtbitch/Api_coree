using Api_coree.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Api_coree.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductoService _productoService;

        public HomeController(ILogger<HomeController> logger, ProductoService productoService)
        {
            _logger = logger;
            _productoService = productoService;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.GetProductosAsync();
            return View("/Views/Producto/Index.cshtml", productos);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
