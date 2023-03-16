using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes.Data.Usuarios;
using NetKubernetes.Dtos.UsuariosDtos;
using NetKubernetes.Models;

namespace NetKubernetes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioResponseDto>> Login([FromBody] UsuarioLoginRequestDto request) 
        { 
             return await _usuarioRepository.Login(request);
        }

        [AllowAnonymous]
        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioResponseDto>> registrar([FromBody] UsuarioRegistroRequestDto request)
        {
            return await _usuarioRepository.ResgistroUsuario(request);
        }


        [HttpGet]
        public async Task<ActionResult<UsuarioResponseDto>> DevolverUsuario()
        {
            return await _usuarioRepository.GetUsuario();
        }

    }
}
