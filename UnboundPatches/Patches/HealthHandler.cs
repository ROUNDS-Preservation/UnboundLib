using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Unbound.Core;
using Unbound.Core.Extensions;

namespace Unbound.Patches {
    [HarmonyPatch(typeof(HealthHandler), "RPCA_Die")]
    class HealthHandler_Patch_RPCA_Die {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            var f_PlayerID = typeof(Player).GetFieldInfo("PlayerID");
            var m_colorID = typeof(PlayerExtensions).GetMethodInfo(nameof(PlayerExtensions.colorID));

            List<CodeInstruction> ins = instructions.ToList();

            int idx = -1;

            for(int i = 0; i < ins.Count(); i++) {
                // we only want to change the first occurence here
                if(!(ins[i].opcode == OpCodes.Callvirt && ins[i].operand.ToString().Contains("Player::get_PlayerID()"))) continue;
                idx = i;
                break;
            }
            if(idx == -1) {
                throw new Exception("[RPCA_Die PATCH] INSTRUCTION NOT FOUND");
            }
            // get colorID instead of PlayerID
            ins[idx] = new CodeInstruction(OpCodes.Call, m_colorID);

            return ins.AsEnumerable();
        }
    }
    [HarmonyPatch(typeof(HealthHandler), "RPCA_Die_Phoenix")]
    class HealthHandler_Patch_RPCA_Die_Phoenix {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            var f_PlayerID = typeof(Player).GetFieldInfo("PlayerID");
            var m_colorID = typeof(PlayerExtensions).GetMethodInfo(nameof(PlayerExtensions.colorID));

            List<CodeInstruction> ins = instructions.ToList();

            int idx = -1;

            for(int i = 0; i < ins.Count(); i++) {
                // we only want to change the first occurence here
                if(!(ins[i].opcode == OpCodes.Callvirt && ins[i].operand.ToString().Contains("Player::get_PlayerID()"))) continue;
                idx = i;
                break;
            }
            if(idx == -1) {
                throw new Exception("[RPCA_Die_Phoenix PATCH] INSTRUCTION NOT FOUND");
            }
            // get colorID instead of PlayerID
            ins[idx] = new CodeInstruction(OpCodes.Call, m_colorID);

            return ins.AsEnumerable();
        }
    }

    [HarmonyPatch(typeof(HealthHandler), "Revive")]
    class HealthHandler_Patch_Revive {
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
