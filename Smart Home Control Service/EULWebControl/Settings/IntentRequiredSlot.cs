using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl
{
    public class IntentRequiredSlot
    {
        public string IntentName { get; set; }
        public List<RequiredSlotWithPrompt> RequiredSlots { get; set; }
    }
}
