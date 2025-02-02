﻿using Autofac;

namespace JG.Infrastructure.Correlation
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CorrelationContextAccessor>().As<ICorrelationContextAccessor>().SingleInstance();
            builder.RegisterType<CorrelationContextFactory>().As<ICorrelationContextFactory>().InstancePerLifetimeScope();
        }
    }
}
