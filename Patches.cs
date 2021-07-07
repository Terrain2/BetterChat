using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace BetterChat
{
    [HarmonyPatch]
    public class ChatFixPatches
    {
        static List<string> history = new List<string>();
        static int selectedMessage = -1;

        [HarmonyPatch(typeof(ChatBox), nameof(ChatBox.SendMessage)), HarmonyPrefix]
        static bool SendMessagePrefix(ChatBox __instance, out int __state, string message)
        {
            __state = __instance.maxMsgLength;
            if (string.IsNullOrEmpty(message))
            {
                __instance.typing = false;
                __instance.ClearMessage();
                __instance.CancelInvoke(nameof(ChatBox.HideChat));
                __instance.Invoke(nameof(ChatBox.HideChat), 5f);
                return false;
            }
            if (history.Count == 0 || history[0] != message) history.Insert(0, message);
            if (message[0] == '/') __instance.maxMsgLength = int.MaxValue;
            return true;
        }

        [HarmonyPatch(typeof(ChatBox), nameof(ChatBox.SendMessage)), HarmonyPostfix]
        static void SendMessagePostfix(ChatBox __instance, int __state)
        {
            __instance.maxMsgLength = __state;
        }

        [HarmonyPatch(typeof(ChatBox), nameof(ChatBox.AppendMessage)), HarmonyPrefix]
        static void AppendMessagePrefix(ChatBox __instance, out int __state, int fromUser)
        {
            __state = __instance.maxChars;
            if (fromUser < 0) __instance.maxChars = int.MaxValue;
        }

        [HarmonyPatch(typeof(ChatBox), nameof(ChatBox.AppendMessage)), HarmonyPostfix]
        static void AppendMessagePostfix(ChatBox __instance, int __state)
        {
            __instance.maxChars = __state;
        }

        [HarmonyPatch(typeof(ChatBox), nameof(ChatBox.UserInput)), HarmonyPostfix]
        static void UserInput(ChatBox __instance)
        {
            if (!__instance.typing) return;
            var prevMsg = selectedMessage;

            if (selectedMessage >= 0 && __instance.inputField.text != history[selectedMessage]) selectedMessage = -1;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedMessage++;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedMessage--;
            }

            if (selectedMessage >= history.Count) selectedMessage = history.Count - 1;
            if (selectedMessage < 0) selectedMessage = -1;

            if (selectedMessage != -1 && selectedMessage != prevMsg)
            {
                __instance.inputField.text = history[selectedMessage];
            }
        }

        [HarmonyPatch(typeof(Hotbar), nameof(Hotbar.Update)), HarmonyPrefix]
        static bool Update() => !OtherInput.Instance.OtherUiActive();
    }
}