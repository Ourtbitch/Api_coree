using Api_coree.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api_coree.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoService _productoService;

        public ProductoController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.GetProductosAsync();
            return View("/Views/Producto/Index.cshtml", productos);

        }

        public async Task<IActionResult> Detalle(int id)
        {
            Producto producto;

            if (id == 0)
            {
                // Si el id es 0, significa que estamos creando un nuevo producto
                producto = new Producto();
                ViewBag.Accion = "Agregar Nuevo Producto";
            }
            else
            {
                // Si el id es distinto de 0, estamos editando un producto existente
                producto = await _productoService.GetProductoAsync(id);
                ViewBag.Accion = "Editar Producto";

                if (producto == null)
                {
                    return NotFound("Producto no encontrado"); // Muestra un error si el producto no existe
                }
            }

            return View("Detalle", producto); // Pasamos un solo producto a la vista Detalle
        }


        [HttpPost]
        public async Task<IActionResult> GuardarCambios(Producto producto)
        {
            bool resultado;

            if (producto.IdProducto == 0)
            {
                resultado = await _productoService.GuardarProductoAsync(producto);
            }
            else
            {
                resultado = await _productoService.EditarProductoAsync(producto);
            }

            if (resultado)
                return RedirectToAction("Index");

            return View("Error", new { mensaje = "No se pudo guardar o actualizar el producto." });
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int idProducto)
        {
            bool resultado = await _productoService.EliminarProductoAsync(idProducto);

            if (resultado)
                return RedirectToAction("Index");

            return View("Error", new { mensaje = "No se pudo eliminar el producto." });
        }

    }
}
