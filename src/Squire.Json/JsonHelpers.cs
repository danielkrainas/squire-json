namespace Squire.Json
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Squire.Validation;
    using Squire.Decoupled;
    using Microsoft.Practices.ServiceLocation;

    public static class JsonHelpers
    {
        public static void AddLocator(this IList<JsonConverter> converters, IServiceLocator locator)
        {
            converters.VerifyParam("converters").IsNotNull();
            locator.VerifyParam("locator").IsNotNull();
            if (!converters.Any(c => c is IocJsonConverter))
            {
                converters.Add(new IocJsonConverter(locator));
            }
        }
    }
}
