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
            if (pageSize <= 0) pageSize = 8;

            var totalItems = await _context.Peliculas.CountAsync();
            var totalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize);

            var peliculas = await _context.Peliculas
                .OrderBy(p => p.Titulo)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var vm = new rodri_movie_mvc.Models.HomeIndexViewModel
            {
                Peliculas = peliculas,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            return View(vm);
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
