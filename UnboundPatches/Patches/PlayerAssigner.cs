using HarmonyLib;
using Unbound.Core.Extensions;

namespace Unbound.Patches {
    [HarmonyPatch(typeof(PlayerAssigner), "RegisterPlayer")]
    class PlayerAssigner_Patch_CreatePlayer {
        public static void Postfix(CharacterData player) {
            player.player.AssignColorID(player.player.TeamID);
        }
    }
}
