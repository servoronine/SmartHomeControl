using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SmartHomeControl.Devices.Jawbone {
    public class UpBandDataItemConverter : JavaScriptConverter {
        public override IEnumerable<Type> SupportedTypes {
            get {
                return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(UpBandDataItem) }));
            }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer) {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            if (type == typeof(UpBandDataItem)) {
                // Create the instance to deserialize into.
                UpBandDataItem dataItem = null;

                if (dictionary.ContainsKey("action")) {
                    dataItem = new UpBandEvent();
                    UpBandEvent dataItemCasted = (UpBandEvent)dataItem;
                    dataItemCasted.action = serializer.ConvertToType<string>(dictionary["action"]);
                    dataItemCasted.tz = serializer.ConvertToType<string>(dictionary["tz"]);
                    dataItemCasted.date = serializer.ConvertToType<int>(dictionary["date"]);
                    dataItemCasted.time_created = serializer.ConvertToType<int>(dictionary["time_created"]);
                }

                return dataItem;
            }
            return null;
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer) {
            throw new NotImplementedException();
        }
    }
}
