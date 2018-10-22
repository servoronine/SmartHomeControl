using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Models.Amazon
{
    public class AmazonResponse
    {
        public string version { get; set; }
        public Dictionary<string, object> sessionAttributes;
        public AmazonResponseObject response;

        public RequiredSlotWithPrompt CreateAttributesFromSlots(Dictionary<string, AmazonSlot> slots, List<RequiredSlotWithPrompt> reqSlots) {
            bool nextSlotFound = false;
            RequiredSlotWithPrompt nextMissingSlot = null;
            sessionAttributes = new Dictionary<string, object>();
            foreach (KeyValuePair<string, AmazonSlot> slot in slots) {
                if (slot.Value.name.StartsWith("PAR")) {
                    sessionAttributes.Add(slot.Value.name, slot.Value.value);
                    if (slot.Value.value == null && !nextSlotFound) {
                        sessionAttributes.Add("NextSlot", AmazonSession.CreateNextSlotString(slot.Value.name));

                        foreach (RequiredSlotWithPrompt sl in reqSlots) {
                            if (sl.RequiredSlot == slot.Value.name) {
                                nextMissingSlot = sl;
                            }
                        }
                        nextSlotFound = true;
                    }
                }
                else {
                    sessionAttributes.Add(slot.Value.name, slot.Value.value);
                }
            }
            return nextMissingSlot;
        }

        public void CopySessionAttributesFromRequest(AmazonRequest request) {
            sessionAttributes = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> kv in request.session.attributes) {
                sessionAttributes.Add(kv.Key, kv.Value);
            }
        }
    }
}
