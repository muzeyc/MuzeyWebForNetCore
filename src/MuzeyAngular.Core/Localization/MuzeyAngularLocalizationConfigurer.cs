using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace MuzeyAngular.Localization
{
    public static class MuzeyAngularLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(MuzeyAngularConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(MuzeyAngularLocalizationConfigurer).GetAssembly(),
                        "MuzeyAngular.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
