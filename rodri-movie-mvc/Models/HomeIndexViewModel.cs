using System.Collections.Generic;

namespace rodri_movie_mvc.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;
    }
}
