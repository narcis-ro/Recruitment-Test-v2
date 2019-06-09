using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JG.Infrastructure.Logging.Configuration;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace JG.Infrastructure.Logging.Extensions
{
    public static class LogSettingsExtensions
    {
        public static void LogStartConfiguration(this ILogger logger, IConfiguration configuration = default,
            bool jgEnvVars = true)
        {
            if (configuration != default)
            {
                var configurationDictionary = configuration.AsEnumerable().ToDictionary(x => x.Key, y => y.Value);
                var secrets = configuration.GetSection(LogProps.APP_SETTING_SECRETS_KEY)?.Get<List<string>>();
                secrets?.ForEach(key => configurationDictionary.Remove(key));

                // TODO: Customize dumping to not include sensitive data
                logger.ForContext(LogProps.APP_SETTINGS, configurationDictionary, true)
                    .Information("AppSettings dump. See props for details.");

                var enrich = new EnrichLoggingConfiguration();
                // TODO: Move to Constants
                configuration.Bind("JG-Logging:Enrich", enrich);

                var props = new Dictionary<string, string>
                {
                    [LogProps.DEVOPS_ENV_NAME] = enrich.DevOpsEnvName,
                    [LogProps.BRANCH] = enrich.Branch,
                    [LogProps.COMMIT] = enrich.Commit,
                    [LogProps.BUILD] = enrich.Build,
                    [LogProps.BUILD_NAME] = enrich.BuildName,
                    [LogProps.BUILT_BY] = enrich.BuiltBy,
                    [LogProps.RELEASE] = enrich.Release,
                    [LogProps.RELEASE] = enrich.Release,
                    [LogProps.RELEASE_NAME] = enrich.ReleaseName,
                    [LogProps.RELEASE_BY] = enrich.ReleaseBy
                };


                logger.ForContext(LogProps.DEV_OPS, props, true)
                    .Information("DevOps configuration. See props for details.");
            }

            if (jgEnvVars)
            {
                IDictionary<string, object> inaSettings = new Dictionary<string, object>();

                foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
                {
                    var key = entry.Key.ToString();

                    if (key.StartsWith("JG_"))
                        inaSettings.Add(key, entry.Value);
                }

                logger.ForContext(LogProps.JG_ENV_VARS, inaSettings, true)
                    .Information("JG environment variables dump. See props for details.");
            }
        }
    }
}
