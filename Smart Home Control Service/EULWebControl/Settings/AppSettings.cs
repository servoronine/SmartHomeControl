using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl
{
    public class AppSettings
    {
        public string AmazonUri { get; set; }
        public string AmazonClientSecret { get; set; }
        public string AmazonClientID { get; set; }
        public string AmazonAccessToken { get; set; }
        public List<IntentRequiredSlot> IntentRequiredSlots { get; set; }
        public string SmartHomeServerEndPointUrl { get; set; }
        public string SmartHomeServerAccessToken { get; set; }

        public List<RequiredSlotWithPrompt> FindRequiredSlotsByIntent(string intentName) {
            List<RequiredSlotWithPrompt> reqSlots = null;
            int delIndex = intentName.IndexOf("Incomplete");
            if (delIndex == -1) {
                delIndex = intentName.IndexOf("FollowUp");
            }
            string intentSearchString = intentName;
            if (delIndex > -1) {
                intentSearchString = intentName.Remove(delIndex);
            }
            foreach (IntentRequiredSlot sl in IntentRequiredSlots) {
                if (sl.IntentName == intentSearchString) {
                    reqSlots = sl.RequiredSlots;
                }
            }
            return reqSlots;
        }
    }
}
