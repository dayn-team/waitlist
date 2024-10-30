﻿using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;

namespace Core.Application.DI {
    public static class Resolve {
        public static void resolve(this IServiceCollection services) {
            services.RegisterAssemblyPublicNonGenericClasses()
              .AsPublicImplementedInterfaces();
        }
    }
}
