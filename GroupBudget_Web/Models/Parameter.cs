using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GroupBudget_Web.Data;
using GB_Web.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace GroupBudget_Web.Models
{
    public class Parameter
    {
        [Key]
        [Display(Name = "Parameter")]
        public string Name { get; set; }

        [Display(Name = "Waarde")]
        public string Value { get; set; }

        [Display(Name = "Beschrijving")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [ForeignKey("GroupBudgetUser")]
        public string UserId { get; set; }

        public DateTime LastChanged { get; set; } = DateTime.Now;

        public DateTime Obsolete { get; set; } = DateTime.MaxValue;

        public string Destination { get; set; }



        public static Dictionary<string, Parameter> Parameters = new Dictionary<string, Parameter>();


        public static void AddParameters(ApplicationDbContext context, GroupBudgetUser user)
        {
            if (!context.Parameters.Any())
            {
                context.Parameters.AddRange(
                    new Parameter { Name = "Version", Value = "0.1.0", Description = "Huidige versie van de parameterlijst", Destination = "System", UserId = user.Id },
                    new Parameter { Name = "Mail.Server", Value = "ergens.groupspace.be", Description = "Naam van de gebruikte pop-server", Destination = "Mail", UserId = user.Id },
                    new Parameter { Name = "Mail.Port", Value = "25", Description = "Poort van de smtp-server", Destination = "Mail", UserId = user.Id },
                    new Parameter { Name = "Mail.Account", Value = "SmtpServer", Description = "Acount-naam van de smtp-server", Destination = "Mail", UserId = user.Id },
                    new Parameter { Name = "Mail.Password", Value = "xxxyyy!2315", Description = "Wachtwoord van de smtp-server", Destination = "Mail", UserId = user.Id },
                    new Parameter { Name = "Mail.Security", Value = "true", Description = "Is SSL or TLS encryption used (true or false)", Destination = "Mail", UserId = user.Id },
                    new Parameter { Name = "Mail.SenderEmail", Value = "administrator.groupspace.be", Description = "E-mail van de smtp-verzender", Destination = "Mail", UserId = user.Id },
                    new Parameter { Name = "Mail.SenderName", Value = "Administrator", Description = "Naam van de smtp-verzender", Destination = "Mail", UserId = user.Id }
                );
                context.SaveChanges();
            }

            foreach (Parameter parameter in context.Parameters)
                Parameters[parameter.Name] = parameter;
            ConfigureMail();
        }

        public static void ConfigureMail()
        {
            MailKitEmailSender mailSender = (MailKitEmailSender) Globals.App.Services.GetService<IEmailSender>();
            var options = mailSender.Options;
            options.Server = Parameters["Mail.Server"].Value;
            options.Server = Parameters["Mail.Server"].Value;
            options.Port = Convert.ToInt32(Parameters["Mail.Port"].Value);
            options.Account = Parameters["Mail.Account"].Value;
            options.Password = Parameters["Mail.Password"].Value;
            options.SenderEmail = Parameters["Mail.SenderEmail"].Value;
            options.SenderName = Parameters["Mail.SenderName"].Value;
            options.Security = Convert.ToBoolean(Parameters["Mail.Security"].Value);
        }
    }
}
