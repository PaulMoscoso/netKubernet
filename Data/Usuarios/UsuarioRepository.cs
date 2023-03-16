using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetKubernetes.Dtos.UsuariosDtos;
using NetKubernetes.Middleware;
using NetKubernetes.Models;
using NetKubernetes.Token;
using System.Net;

namespace NetKubernetes.Data.Usuarios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IJwtGenerador _jwtGenerador;
        private readonly AppDbContext _appContext;
        private readonly IUsuarioSesion _usuarioSesion;

        public UsuarioRepository(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador, AppDbContext appContext, IUsuarioSesion usuarioSesion)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerador = jwtGenerador;
            _appContext = appContext;
            _usuarioSesion = usuarioSesion;
        }

        private UsuarioResponseDto TransformerUserToUseDto(Usuario usuario) {
            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Telefono = usuario.Telefono,
                Email = usuario.Email,
                UserName = usuario.UserName,
                Token = _jwtGenerador.CrearToken(usuario)
            };
        }

        public async Task<UsuarioResponseDto> GetUsuario()
        {
            var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());

            if (usuario is null) {
                throw new MiddlewareException(HttpStatusCode.Unauthorized, new {mensaje = "El usuario del token non existe en la base de datos"});
            }

            var usuarioDto = TransformerUserToUseDto(usuario!);
            return usuarioDto;
        }

        public async Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto request)
        {
            var usuario = await _userManager.FindByEmailAsync(request.Email);
            if (usuario is null)
            {
                throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "El Email non ha sido encontrado en la base de datos" });
            }

            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario!, request.Password, false);

            if (resultado.Succeeded) {
                return TransformerUserToUseDto(usuario);
            }

            throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "Las credenciales son incorrectas" });
            //return TransformerUserToUseDto(usuario);
        }

        public async Task<UsuarioResponseDto> ResgistroUsuario(UsuarioRegistroRequestDto request)
        {
            var existEmail = await _appContext.Users.Where(x => x.Email.Equals(request.Email)).AnyAsync();

            if (existEmail) {
                throw new MiddlewareException(HttpStatusCode.BadRequest, new { mensaje = "El email ya existe" });
            }

            var existUserName = await _appContext.Users.Where(x => x.UserName.Equals(request.UserName)).AnyAsync();

            if (existUserName)
            {
                throw new MiddlewareException(HttpStatusCode.BadRequest, new { mensaje = "El userName ya existe" });
            }

            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Telefono = request.Telefono,
                Email = request.Email,
                UserName = request.UserName,
            };

            var resultado = await _userManager.CreateAsync(usuario!, request.password);
            if (resultado.Succeeded) { 
                return TransformerUserToUseDto(usuario);
            }
            throw new Exception("No se pudo registrar el usuario");
        }
    }
}
