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
        public async Task<IActionResult> Index(int paginaActual = 1, int pageSize = 8, string txtBusqueda = "", int generoId = 0, int plataformaId = 0) 
        {
            if (paginaActual < 1) paginaActual = 1;
            if (pageSize != 8) pageSize = 8;


            var consulta = _context.Peliculas.AsQueryable();
            if (!string.IsNullOrEmpty(txtBusqueda))
            {
                consulta = consulta.Where(p => p.Titulo.Contains(txtBusqueda));
            }

            if (generoId != 0)
            {
                consulta = consulta.Where(p => p.GeneroId == generoId);
            }

            if (plataformaId != 0)
            {
                consulta = consulta.Where(p => p.PlataformaId == plataformaId);
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

            var generos = await _context.Generos.OrderBy(g => g.Descripcion).ToListAsync();
            generos.Insert(0, new Genero { Id = 0, Descripcion = "Todos" });
            ViewBag.GeneroId = new SelectList(generos, "Id", "Descripcion", generoId);

            var plataforma = await _context.Plataformas.OrderBy(p => p.Nombre).ToListAsync();
            plataforma.Insert(0, new Plataforma { Id = 0, Nombre = "Todos" });
            ViewBag.PlataformaId = new SelectList(plataforma, "Id", "Nombre", plataformaId);

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
