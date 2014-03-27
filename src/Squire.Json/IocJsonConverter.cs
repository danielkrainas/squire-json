namespace Squire.Json
{
    using Microsoft.Practices.ServiceLocation;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Squire.Validation;

    public class IocJsonConverter : JsonConverter
    {
        private readonly IServiceLocator locator;

        public IocJsonConverter(IServiceLocator locator)
        {
            locator.VerifyParam("locator").IsNotNull();
            this.locator = locator;
        }

        public override bool CanConvert(Type objectType)
        {
            if(!objectType.IsClass)
            {
                return false;
            }

            return this.locator.GetAllInstances<JsonConverter>().Any(c => c.CanConvert(objectType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            foreach (var converter in this.locator.GetAllInstances<JsonConverter>().Where(c => c.CanConvert(objectType)))
            {
                existingValue = converter.ReadJson(reader, objectType, existingValue, serializer);
            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            foreach (var converter in this.locator.GetAllInstances<JsonConverter>().Where(c => value == null || c.CanConvert(value.GetType())))
            {
                converter.WriteJson(writer, value, serializer);
            }
        }
    }
}
