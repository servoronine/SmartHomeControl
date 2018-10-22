using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Models.Amazon
{
    public class AmazonSession
    {
        public bool @new { get; set; }
        public string sessionId { get; set; }
        public AmazonApplication application { get; set; }
        public Dictionary<string, object> attributes { get; set; }
        public AmazonUser user { get; set; }

        public void SortSessionAttributes(List<RequiredSlotWithPrompt> reqSlots) {
            Dictionary<string, object> sortedAttributes = new Dictionary<string, object>();
            foreach (RequiredSlotWithPrompt slNeeded in reqSlots) {
                foreach (KeyValuePair<string, object> kv in attributes) {
                    if (kv.Key == slNeeded.RequiredSlot) {
                        sortedAttributes.Add(kv.Key, kv.Value);
                        continue;
                    }
                }
            }
            attributes = sortedAttributes;
        }

        public List<AmazonSlot> ConvertSessionAttributesToSlots() {
            List<AmazonSlot> slotsPresent = new List<AmazonSlot>();
            foreach (KeyValuePair<string, object> attribute in attributes) {
                AmazonSlot slot = new AmazonSlot();
                slot.name = attribute.Key;
                slot.value = attribute.Value.ToString();
                slotsPresent.Add(slot);
            }
            return slotsPresent;
        }

        public RequiredSlotWithPrompt CheckIfAllSlotsArePresent(List<RequiredSlotWithPrompt> reqSlots) {
            foreach (KeyValuePair<string, object> kv in attributes) {
                if (kv.Value == null) {
                    foreach (RequiredSlotWithPrompt sl in reqSlots) {
                        if (sl.RequiredSlot == kv.Key) {
                            attributes["NextSlot"] = CreateNextSlotString(kv.Key);
                            return sl;
                        }
                    }
                }
            }
            return null;
        }

        public static string CreateNextSlotString(string rawString) {
            int delIndex = rawString.IndexOf("Date");
            if (delIndex == -1) {
                delIndex = rawString.IndexOf("Int");
            }
            if (delIndex == -1) {
                delIndex = rawString.IndexOf("Time");
            }
            rawString = rawString.Remove(delIndex);
            rawString = rawString.Substring(3);
            return rawString;
        }
    }
}
