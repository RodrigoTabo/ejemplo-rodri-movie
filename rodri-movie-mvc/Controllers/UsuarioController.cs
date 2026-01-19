using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using rodri_movie_mvc.Models;
using rodri_movie_mvc.Service;
using rodri_movie_mvc.ViewModels;
using System.Threading.Tasks;

namespace rodri_movie_mvc.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ImagenStorage _imagenStorage;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ImagenStorage imagenStorage)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _imagenStorage = imagenStorage;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Password, usuario.RememberMe, lockoutOnFailure: false);
                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
                }
            }
            return View(usuario);
        }

        public IActionResult Registro()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                var nuevoUsuario = new Usuario
                {
                    UserName = usuario.Email,
                    Email = usuario.Email,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    ImagenUrlPerfil = "default-profile.png"
                };
                var resultado = await _userManager.CreateAsync(nuevoUsuario, usuario.Password);
                if (resultado.Succeeded)
                {
                    await _signInManager.SignInAsync(nuevoUsuario, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(usuario);
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> MiPerfil()
        {
            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null)
                return RedirectToAction("Login", "Usuario");

            var MiPerfilViewModel = new MiPerfilViewModel
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                ImagenUrlPerfil = usuario.ImagenUrlPerfil
            };

            return View(MiPerfilViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MiPerfil(MiPerfilViewModel MiPerfil)
        {

            if (ModelState.IsValid)
            {
                var usuarioActual = await _userManager.GetUserAsync(User);


                try
                {
                    if(MiPerfil.ImagenPerfil is not null && MiPerfil.ImagenPerfil.Length > 0)
                    {
                        //opcional; borrar la anterior si no es placeholder.
                        if(!string .IsNullOrEmpty(usuarioActual.ImagenUrlPerfil))
                            await _imagenStorage.DeleteAsync(usuarioActual.ImagenUrlPerfil);

                        var nuevaRuta = await _imagenStorage.SaveAsync(usuarioActual.Id, MiPerfil.ImagenPerfil);
                        usuarioActual.ImagenUrlPerfil = nuevaRuta;
                    }

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(MiPerfil);
                }



                usuarioActual.Nombre = MiPerfil.Nombre;
                usuarioActual.Apellido = MiPerfil.Apellido;

                var resultado = await _userManager.UpdateAsync(usuarioActual);

                if (resultado.Succeeded)
                {
                    ViewBag.Mensaje = "Perfil actualizado correctamente.";
                    return View(MiPerfil);
                }
                else
                {
                    foreach (var error in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }


            return View(MiPerfil);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }



    }
}
