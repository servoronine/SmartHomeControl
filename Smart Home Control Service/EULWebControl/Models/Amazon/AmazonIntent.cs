using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeWebControl.Models.Amazon
{
    public class AmazonIntent
    {
        public string name { get; set; }
        public Dictionary<string, AmazonSlot> slots { get; set; }

        public void SortSlots(List<RequiredSlotWithPrompt> reqSlots) {
            Dictionary<string, AmazonSlot> sortedSlots = new Dictionary<string, AmazonSlot>();
            foreach (RequiredSlotWithPrompt slNeeded in reqSlots) {
                foreach (KeyValuePair<string, AmazonSlot> kv in slots) {
                    if (kv.Key == slNeeded.RequiredSlot) {
                        sortedSlots.Add(kv.Key, kv.Value);
                        continue;
                    }
                }
            }
            slots = sortedSlots;
        }

        public void ReplaceSlots(List<AmazonSlot> slots) {
            this.slots.Clear();
            foreach (AmazonSlot sl in slots) {
                this.slots.Add(sl.name, sl);
            }
        }

        public void AssignParameterTypeToSlotValues() {
            if (slots == null) return;

            foreach (AmazonSlot slot in slots.Values) {
                if (slot.name.StartsWith("PAR")) {
                    if (slot.name == "!!PROCESSED") continue;

                    if (slot.name.Contains("Int")) {
                        slot.value = "int|" + slot.value;
                    }
                    if (slot.name.Contains("Str")) {
                        slot.value = "string|" + slot.value;
                    }
                    if (slot.name.Contains("Date")) {
                        string timeSearchString = slot.name.Substring(0, slot.name.Length - 4) + "Time";
                        slot.value = "date|" + slot.value;

                        AmazonSlot timeSlot = null;
                        if (slots.ContainsKey(timeSearchString)) {
                            timeSlot = slots[timeSearchString];
                        }
                        if (timeSlot != null) {
                            slot.value = slot.value + " " + timeSlot.value;
                            timeSlot.name = "!!PROCESSED";
                        }
                        else {
                            slot.value = slot.value + " 00:00";
                        }
                    }
                }
            }
        }

        public ReceivedCommand ConvertIntentToCommand() {
            ReceivedCommand command = new ReceivedCommand();
            if (name.EndsWith("FollowUp")) {
                command.Remote = name.Substring(0, name.Length - 8);
            }
            else {
                command.Remote = name;
            }

            if (command.Remote.Contains("_")) {
                string[] splitCommand = command.Remote.Split('_');
                command.Remote = splitCommand[0];
                command.Command = splitCommand[1];
            }

            string fullVoiceCommand = "<VOICE>";
            if (slots != null) {
                command.Parameters = new List<string>();
                foreach (AmazonSlot slot in slots.Values) {
                    if (slot.name == "!!PROCESSED") continue;

                    if (slot.name.Contains("PAR")) {
                        command.Parameters.Add(slot.value);
                    }
                    else {
                        if (fullVoiceCommand != "<VOICE>") {
                            fullVoiceCommand = fullVoiceCommand += " ";
                        }
                        fullVoiceCommand += slot.value;
                    }
                }
            }
            if (command.Command == null) {
                command.Command = fullVoiceCommand;
            }
            return command;
        }

        public void RemoveNullValueSlots() {
            Dictionary<string, AmazonSlot> slotsWithoutNulls = new Dictionary<string, AmazonSlot>();
            if (slots != null) {
                foreach (KeyValuePair<string, AmazonSlot> kv in slots) {
                    if (kv.Value.value != null) {
                        slotsWithoutNulls.Add(kv.Key, kv.Value);
                    }
                }
                slots = slotsWithoutNulls;
            }
        }
    }
}
