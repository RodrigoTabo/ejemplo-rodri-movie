using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rodri_movie_mvc.Data;
using rodri_movie_mvc.Models;

namespace rodri_movie_mvc.Controllers
{
    public class HomeController : Controller
    {

        private readonly MovieDbContext _context;

        public HomeController(MovieDbContext context)
        {
                _context = context;
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
        {
            if (page < 1) page = 1;
            if (pageSize != 8) pageSize = 8;

            var totalItems = await _context.Peliculas.CountAsync();
            var totalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize);

            if (page > totalPages && totalPages > 0) page = totalPages;

            var peliculas = await _context.Peliculas
                .OrderBy(p => p.Titulo)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;

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
