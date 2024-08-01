using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_JWT.Custom;
using WebApi_JWT.Models;
using WebApi_JWT.Models.DTOs;

namespace WebApi_JWT.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly PruebaJwtContext _jwtContext;
        private readonly Utilidades _utlidades;
        public AccesoController(PruebaJwtContext jwtContext, Utilidades utilidades)
        {
            _jwtContext = jwtContext;
            _utlidades = utilidades;
        }

        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(UsuarioDTO objeto)
        {
            var modeloUsuario = new Usuario
            {
                Nombre = objeto.Nombre,
                Correo = objeto.Correo,
                Clave = _utlidades.EncriptarSHA256(objeto.Clave)
            };

            await _jwtContext.Usuarios.AddAsync(modeloUsuario);
            await _jwtContext.SaveChangesAsync();

            if (modeloUsuario.IdUsuario != 0)
            {
                return StatusCode(StatusCodes.Status200OK, new {isSucces = true});
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { isSucces = false });
            }
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO objeto)
        {
            var usuarioEncontrado = await _jwtContext.Usuarios.
                Where(u => 
                u.Correo == objeto.Correo &&
                u.Clave == _utlidades.EncriptarSHA256(objeto.Clave)
                ).FirstOrDefaultAsync();

            if (usuarioEncontrado == null)
            {
                return StatusCode(StatusCodes.Status200OK, new { isSucces = false, token = "" });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { isSucces = true, token = _utlidades.GenerarJWT(usuarioEncontrado)});

            }
        }
    }
}
