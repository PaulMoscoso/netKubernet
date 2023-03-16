using Microsoft.AspNetCore.Identity;
using NetKubernetes.Models;

namespace NetKubernetes.Data
{
    public class LoadDatabase
    {
        public static async Task InsertarData(AppDbContext context, UserManager<Usuario> usuarioManager) {
            if (!usuarioManager.Users.Any()) {
                var usuario = new Usuario
                {
                    Nombre = "Paul",
                    Apellido = "Moscoso",
                    Email = "pauluni107@hotmail.com",
                    UserName ="paul.moscoso",
                    Telefono ="3434543"
                };

                await  usuarioManager.CreateAsync(usuario, "PasswordPaul123$");
            }

            if (!context.Inmuebles.Any()) {
                await context.Inmuebles.AddRangeAsync(
                        new Inmueble
                        {
                            Nombre = "Casa del playa",
                            Direccion = "Av El sol 32",
                            Precio = 4500M,
                            FechaCreacion = DateTime.Now

                        },
                        new Inmueble
                        {
                            Nombre = "Casa de invierno",
                            Direccion = "Av La Roca 101",
                            Precio = 3500M,
                            FechaCreacion = DateTime.Now

                        }
                    );
            }

            await context.SaveChangesAsync();

         }

    }
}
