using Api_coree.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
                // Crear un nuevo producto si el id es 0 (para agregar un producto nuevo)
                producto = new Producto();
                ViewBag.Accion = "Agregar Nuevo Producto";
            }
            else
            {
                // Obtener el producto existente para editar si el id no es 0
                producto = await _productoService.GetProductoAsync(id);

                if (producto == null)
                {
                    return View("Error", new { mensaje = "El producto no existe." });
                }

                // Imprime los datos del producto para ver si contiene información antes de pasarlo a la vista
                Console.WriteLine("Producto para editar: " + JsonConvert.SerializeObject(producto));

                ViewBag.Accion = "Editar Producto";
            }

            return View("Detalle", producto);
        }






        [HttpPost]
        public async Task<IActionResult> GuardarCambios(Producto producto)
        {
            bool resultado;

            if (producto.IdProducto == 0)
            {
                // Crear nuevo producto
                resultado = await _productoService.GuardarProductoAsync(producto);
            }
            else
            {
                // Editar producto existente
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
