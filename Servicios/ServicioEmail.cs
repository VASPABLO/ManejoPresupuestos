﻿using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace ManejoPresupuestos.Servicios
{
    public interface IServicioEmail
    {
        Task EnviarEmailCambioPassword(string receptor, string enlace);
    }

    public class ServicioEmail : IServicioEmail
    {
        private readonly IConfiguration configuration;

        public ServicioEmail(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task EnviarEmailCambioPassword(string receptor, string enlace)
        {
            var email = configuration.GetValue<string>("CONFIGURACIONES_HOTMAIL:EMAIL");
            var password = configuration.GetValue<string>("CONFIGURACIONES_HOTMAIL:PASSWORD");
            var host = configuration.GetValue<string>("CONFIGURACIONES_HOTMAIL:HOST");
            var puerto = configuration.GetValue<int>("CONFIGURACIONES_HOTMAIL:PUERTO");


            var cliente = new SmtpClient(host, puerto);
            cliente.EnableSsl = true;
            cliente.UseDefaultCredentials = false;

            cliente.Credentials = new NetworkCredential(email, password);

            var emisor = email;
            var tema = "¿Ha olvidado su contraseña?";

            var contenidoHTML = $@"Saludos,
Este mensaje le llega porque usted ha solicitado un cambio de contraseña. Si esta solicitud no fue realizada por ustes, puede ignorar este mensaje.
Para cambiar su contraseña, haga click en el siguiente enlace:

{enlace}

Atentamente,
Equipo de Manejo de presupuesto.";

            var mensajeEmail = new MailMessage(emisor, receptor, tema, contenidoHTML);
            await cliente.SendMailAsync(mensajeEmail);
        }
    }
}
