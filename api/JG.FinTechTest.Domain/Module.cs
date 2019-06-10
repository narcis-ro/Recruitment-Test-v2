using Autofac;
using JG.FinTechTest.Domain.Config;
using JG.FinTechTest.Domain.TaxEngine;
using JG.FinTechTest.Domain.TaxProcessors;
using LiteDB;
using Microsoft.Extensions.Configuration;

namespace JG.FinTechTest.Domain
{
    public class Module : Autofac.Module
    {
        public string DonationConfigKey { get; set; } = "Donation";

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new Infrastructure.Cqs.Module(ThisAssembly));
            builder.RegisterModule(new Infrastructure.MediatR.Module(ThisAssembly));

            builder.RegisterType<ApplicableTaxSelector>().As<IApplicableTaxSelector>().SingleInstance();
            builder.RegisterType<TaxProcessorFactory>().As<ITaxProcessorFactory>().SingleInstance();

            builder.Register(c =>
            {
                // TODO: Validate settings. Use a startup task if needed.
                var configuration = c.Resolve<IConfiguration>();
                var donationConfig = new DonationConfig();

                // Note: If we require hot reloading, there are other ways to do it.
                configuration.Bind(DonationConfigKey, donationConfig);

                return donationConfig;
            }).AsSelf().SingleInstance();

            builder.Register(c => new LiteDatabase(@"MyData.db")).As<LiteDatabase>().SingleInstance();
        }
    }
}
