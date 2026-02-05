using rodri_movie_mvc.Models;
using System.ComponentModel.DataAnnotations;

namespace rodri_movie_mvc.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public string? PeliculaTitulo { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        [Range(1, 5, ErrorMessage ="La calificación deber ser entre 1 y 5 estrellas")]
        [Required(ErrorMessage = "La calificación es requerida.")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "El comentario es requerido.")]
        [MaxLength(500, ErrorMessage ="El comentario no puede exceder mas de 500 caracteres.")]
        public string Comentario { get; set; }

        public DateTime Fecha { get; set; }


    }
}
