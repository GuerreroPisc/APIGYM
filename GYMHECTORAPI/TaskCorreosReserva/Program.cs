using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using GYMHECTORAPI.Bussiness;
using GYMHECTORAPI.DataAccess;
using GYMHECTORAPI.Models.GTMHECTOR.DB;
using System.Net.Mail;
using System.Net;
using System.Reflection;

class Program
{
    static async Task Main(string[] args)
    {
        // Cargar configuración desde appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();

        // Logging
        services.AddLogging(config => config.AddConsole());

        // Registrar el DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<GymHectorContext>(options =>
            options.UseSqlServer(connectionString));

        // Registrar capas
        services.AddScoped<IUsuarioDO, UsuarioDO>();
        services.AddScoped<IUsuarioBO, UsuarioBO>();

        // Registrar configuración SMTP
        services.Configure<SmtpCorreoOptions>(configuration.GetSection("SmtpCorreo"));

        // Crear provider
        var serviceProvider = services.BuildServiceProvider();

        // Obtener datos del usuario
        var usuarioBO = serviceProvider.GetRequiredService<IUsuarioBO>();
        var resultado = await usuarioBO.ListarMaestros(5);

        // Obtener configuración SMTP
        var smtpOptions = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<SmtpCorreoOptions>>().Value;

        // Enviar correo
        using (var mail = new MailMessage())
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string bodyCorreo = @"
                                <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                                  <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 10px; padding: 30px; box-shadow: 0 4px 8px rgba(0,0,0,0.1);'>
                                    <h2 style='color: #2c3e50;'>🚴‍♂️ ¡Hola Augusto!</h2>
                                    <p style='font-size: 16px; color: #333333;'>
                                      Te recordamos que tienes una clase programada para el <strong>día de mañana</strong>:
                                    </p>
                                    <table style='width: 100%; margin-top: 20px; border-collapse: collapse;'>
                                      <tr>
                                        <td style='padding: 10px; background-color: #f0f0f0; border-radius: 5px;'>
                                          <strong>🏋️ Clase:</strong> Bicicleta
                                        </td>
                                      </tr>
                                      <tr>
                                        <td style='padding: 10px; background-color: #f0f0f0; border-radius: 5px; margin-top: 10px;'>
                                          <strong>🕓 Horario:</strong> 4:00 PM a 5:00 PM
                                        </td>
                                      </tr>
                                    </table>
                                    <p style='margin-top: 20px; font-size: 16px; color: #333333;'>
                                      Prepárate para disfrutar de una sesión llena de energía. ¡Nos vemos en el gimnasio!
                                    </p>
                                    <p style='margin-top: 30px; font-size: 14px; color: #888888;'>
                                      Este es un mensaje automático, por favor no respondas a este correo.
                                    </p>
                                  </div>
                                </body>";

            mail.To.Add(new MailAddress("jguerreropisco@outlook.com"));
            mail.From = new MailAddress(smtpOptions.CorreoRemitente, smtpOptions.NombreRemitente);
            mail.Subject = "Prepárate para pedalear 🚴 Mañana 4:00 PM en Gym Hector";
            mail.Body = bodyCorreo;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;

            using (var client = new SmtpClient("smtp.office365.com"))
            {
                client.Port = smtpOptions.PuertoSmtp;
                client.Credentials = new NetworkCredential(smtpOptions.CorreoPrincipal, smtpOptions.PasswordCorreo);
                client.EnableSsl = true;
                client.Send(mail);
            }
        }
    }

    public class SmtpCorreoOptions
    {
        public string CorreoRemitente { get; set; }
        public string NombreRemitente { get; set; }
        public string CorreoPrincipal { get; set; }
        public string PasswordCorreo { get; set; }
        public int PuertoSmtp { get; set; }
    }

}