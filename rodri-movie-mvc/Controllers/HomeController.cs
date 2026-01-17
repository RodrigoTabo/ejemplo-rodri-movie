using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rodri_movie_mvc.Data;
using rodri_movie_mvc.Models;
using System.Diagnostics;

namespace rodri_movie_mvc.Controllers
{
    public class HomeController : Controller
    {

        private readonly MovieDbContext _context;

        public HomeController(MovieDbContext context)
        {
                _context = context;
        }
        public async Task<IActionResult> Index(int paginaActual = 1, int pageSize = 8, string txtBusqueda = "") 
        {
            if (paginaActual < 1) paginaActual = 1;
            if (pageSize != 8) pageSize = 8;


            var consulta = _context.Peliculas.AsQueryable();
            if (!string.IsNullOrEmpty(txtBusqueda))
            {
                consulta = consulta.Where(p => p.Titulo.Contains(txtBusqueda));
            }




            var totalItems = await consulta.CountAsync();
            var totalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize);

            if (paginaActual > totalPages && totalPages > 0) paginaActual = totalPages;

            var peliculas = await consulta
                .OrderBy(p => p.Titulo)
                .Skip((paginaActual - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.PaginaActual = paginaActual;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            ViewBag.TxtBusqueda = txtBusqueda;

            return View(peliculas);
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
