using Microsoft.AspNetCore.Mvc;

namespace SpaWebApp.Controllers
{
    public class NoticiasController : Controller
    {
        public IActionResult Index()
        {
            // Aquí puedes pasar datos o un modelo de noticias desde la base de datos.
            return View();
        }

        public IActionResult Detalle(int id)
        {
                // Títulos y descripciones de ejemplo
                var noticias = new Dictionary<int, (string Titulo, string Descripcion)>
                {
                    { 1, ("Título de la Noticia 1", "Esta es la descripción completa de la noticia 1.") },
                    { 2, ("Título de la Noticia 2", "Esta es la descripción completa de la noticia 2.") },
                    { 3, ("Título de la Noticia 3", "Esta es la descripción completa de la noticia 3.") }
                };

                if (noticias.ContainsKey(id))
                {
                    ViewBag.Titulo = noticias[id].Titulo;
                    ViewBag.Descripcion = noticias[id].Descripcion;
                    ViewBag.Id = id; // Para la imagen de la noticia
                }
                else
                {
                    return NotFound(); // Maneja los casos donde el id no exista
                }

                return View();
        }


    }
}
