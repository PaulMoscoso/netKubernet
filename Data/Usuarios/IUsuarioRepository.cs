using NetKubernetes.Dtos.UsuariosDtos;

namespace NetKubernetes.Data.Usuarios
{
    public interface IUsuarioRepository
    {
        Task<UsuarioResponseDto> GetUsuario();
        Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto request );

        Task<UsuarioResponseDto> ResgistroUsuario(UsuarioRegistroRequestDto request);
    }
}
