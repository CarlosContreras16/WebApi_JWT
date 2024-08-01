using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_JWT.Models;

namespace WebApi_JWT.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly PruebaJwtContext _jwtContext;  
        public ProductoController(PruebaJwtContext jwtContext)
        {
            _jwtContext = jwtContext;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var lista = await _jwtContext.Productos.ToListAsync();
            return StatusCode( StatusCodes.Status200OK, new { value = lista });
        }
    }
}
