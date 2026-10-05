using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity;

namespace AppForSEII.API.Data {
    public class SeedData {
        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger) {
            List<string> rolesNames = new List<string> { "Administrator", "Employee", "Customer" };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            try {
                SeedRoles(roleManager, rolesNames);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the roles in the Database.");
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the Users in the Database.");
            }

            // NUEVO: Inyectar datos de negocio (Accesorios, Impresoras, etc.)
            try {
                SeedBusinessData(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the business data in the Database.");
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles) {
            foreach (string roleName in roles) {
                if (!roleManager.RoleExistsAsync(roleName).Result) {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles) {
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null) {
                ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                }
            }

            if (userManager.FindByNameAsync("peter@uclm.es").Result == null) {
                ApplicationUser user = new ApplicationUser("3", "Peter", "Jackson", "peter@uclm.es");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "OtherPass12$");
                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    userManager.AddToRoleAsync(user, roles[2]).Wait();
                }
            }
        }

        public static void SeedBusinessData(ApplicationDbContext context) {
            // 1. Crear Accesorios si la tabla está vacía
            if (!context.Accesorios.Any()) {
                
                // Creamos una categoría temporal porque es [Required] para el Accesorio
                var categoriaPrueba = new CategoriaAccesorio {
                    // Si esta clase tiene propiedades obligatorias (como Nombre), ponlas aquí. 
                    // Si es un Enum, borra esta variable y asigna el Enum directamente abajo.
                };

                context.Accesorios.AddRange(
                    new Accesorio { 
                        Nombre = "Bobina PLA Rojo",
                        Compatibilidad = "Cualquier impresora FDM",
                        CantidadDisponible = 25,
                        Precio = 19.99m,
                        Categoria = categoriaPrueba
                    },
                    new Accesorio {
                        Nombre = "Boquilla Latón 0.4mm",
                        Compatibilidad = "Creality Ender 3",
                        CantidadDisponible = 100,
                        Precio = 4.50m,
                        Categoria = categoriaPrueba
                    }
                );
            }

            context.SaveChanges();
        }
    }
}