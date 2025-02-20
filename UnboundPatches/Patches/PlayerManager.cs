using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unbound.Core;
using Unbound.Core.Extensions;


namespace Unbound.Patches {
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.GetColorFromPlayer))]
    class PlayerManager_Patch_GetColorFromPlayer {
        static void Prefix(ref int PlayerID) {
            PlayerID = PlayerManager.instance.players[PlayerID].colorID();
        }
    }
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.GetColorFromTeam))]
    class PlayerManager_Patch_GetColorFromTeam {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            var f_PlayerID = typeof(Player).GetFieldInfo("PlayerID");
            var m_colorID = typeof(PlayerExtensions).GetMethodInfo(nameof(PlayerExtensions.colorID));

            foreach(var ins in instructions) {
                if(ins.opcode == OpCodes.Callvirt && ins.operand.ToString().Contains("Player::get_PlayerID()")) {
                    // we want colorID instead of TeamID
                    yield return new CodeInstruction(OpCodes.Call, m_colorID); // call the colorID method, which pops the player instance off the stack and leaves the result [colorID, ...]
                } else {
                    yield return ins;
                }
            }
        }
    }
}
