/*using Microsoft.IdentityModel.Tokens;

namespace SistemaAlertasBackEnd.Utilidades
{
    public static class Llaves
    {
        public const string IssuerPropio = "nuestra-app";
        private const string SeccionLlaves = "Authentication:Schemes:Bearer:SigningKeys";
        private const string SeccionLlaves_Emisor = "Issuer";
        private const string SeccionLlaves_Valor = "Value";

        public static IEnumerable<SecurityKey> ObtenerLlave(IConfiguration configuration) => ObtenerLlave(configuration, IssuerPropio);

        //Para emisores externos como facebook
        public static IEnumerable<SecurityKey> ObtenerLlave(IConfiguration configuration, string issuer)
        {
            var signingKey = configuration.GetSection(SeccionLlaves)
                .GetChildren()
                .SingleOrDefault(llave => llave[SeccionLlaves_Emisor] == issuer);

            if (signingKey is not null && signingKey[SeccionLlaves_Valor] is string valorLlave)
            {
                yield return new SymmetricSecurityKey(Convert.FromBase64String(valorLlave));
            }
        }
        //Metodo para obtener todas las llaves
        public static IEnumerable<SecurityKey> ObtenerTodasLasLlaves(IConfiguration configuration)
        {
            var signingKeys = configuration.GetSection(SeccionLlaves)
                .GetChildren();

            foreach (var signingKey in signingKeys)
            {
                if (signingKey[SeccionLlaves_Valor] is string valorLlave)
                {
                    yield return new SymmetricSecurityKey(Convert.FromBase64String(valorLlave));
                }
            }
        }

    }
}*/

using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SistemaAlertasBackEnd.Utilidades
{
    public static class Llaves
    {
        // Método para obtener la llave JWT desde appsettings.json
        public static IEnumerable<SecurityKey> ObtenerLlave(IConfiguration configuration)
        {
            // Leer la clave desde la sección Jwt:Key
            var llaveSecreta = configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(llaveSecreta))
            {
                throw new InvalidOperationException("No se encontró la clave JWT en la configuración.");
            }

            // Convertir la clave de Base64 a bytes y devolverla como SecurityKey
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(llaveSecreta));
            return new List<SecurityKey> { key };
        }

        // Método para obtener todas las llaves (aunque solo tengas una en este caso)
        public static IEnumerable<SecurityKey> ObtenerTodasLasLlaves(IConfiguration configuration)
        {
            return ObtenerLlave(configuration);
        }
    }
}
