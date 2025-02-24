using DDDProject.Common.Resources;
using Microsoft.Extensions.Localization;

namespace DDDProject.Common.Localization
{
    public class SharedLocalizer
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public SharedLocalizer(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }

        public string this[string key]
        {
            get { return _localizer[key]; }
        }

        public string this[string key, params object[] arguments]
        {
            get { return _localizer[key, arguments]; }
        }
    }
}