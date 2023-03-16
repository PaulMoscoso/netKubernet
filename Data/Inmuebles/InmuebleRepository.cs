using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetKubernetes.Middleware;
using NetKubernetes.Models;
using NetKubernetes.Token;
using System.Net;

namespace NetKubernetes.Data.Inmuebles
{
    public class InmuebleRepository : IInmuebleRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUsuarioSesion _usuarioSesion;
        private readonly UserManager<Usuario> _userManager;

        public InmuebleRepository(AppDbContext appDbContext, IUsuarioSesion usuarioSesion, UserManager<Usuario> userManager)
        {
            _appDbContext = appDbContext;
            _usuarioSesion = usuarioSesion;
            _userManager = userManager;
        }

        public async Task CreateInmueble(Inmueble inmueble)
        {
            var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());

            if (usuario is null) {
                throw new MiddlewareException(
                    HttpStatusCode.Unauthorized,
                    new { mensaje ="El usuario no es valido para hacer esta insercion"}
                    );
            }

            if (inmueble is null) {
                throw new MiddlewareException(
                   HttpStatusCode.BadRequest,
                   new { mensaje = "Los dato no pueden ser nullos" }
                   );
            }

            inmueble.FechaCreacion = DateTime.Now;
            inmueble.UsuarioId = Guid.Parse(usuario.Id);
            _appDbContext.Inmuebles.Add(inmueble);
        }

        public async Task DeleteInmueble(int id)
        {
            var inmueble = await _appDbContext.Inmuebles.FirstOrDefaultAsync(x => x.Id == id);
            _appDbContext.Inmuebles.Remove(inmueble!);
        }

        public async Task<IEnumerable<Inmueble>> GetAllInmuebles()
        {
            var inmuebles = await _appDbContext.Inmuebles.ToListAsync();
            return inmuebles;
        }

        public async Task<Inmueble> GetInmuebleById(int id)
        {
            var inmueble = await _appDbContext.Inmuebles!.FirstOrDefaultAsync(x => x.Id == id)!;
            return inmueble;
        }

        public async Task<bool> SaveChanges()
        {
            return (await _appDbContext.SaveChangesAsync() >= 0) ;
        }
    }
}
