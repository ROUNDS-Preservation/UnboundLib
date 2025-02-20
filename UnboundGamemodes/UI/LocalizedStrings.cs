using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unbound.Core;
using Unbound.Core.Utils;
using UnityEngine.Localization;

namespace Unbound.Gamemodes.UI
{
    public class LocalizedStrings
    {
        private static readonly TableRefHelper translationTable = new TableRefHelper(UnboundCore.ModId)
            .UpdateSrting("VictoryText", new TableRefHelper.TranslationData()
            {
                en_US = "VICTORY!"
            })
            .UpdateSrting("RematchText", new TableRefHelper.TranslationData()
            {
                en_US = "REMATCH?"
            })
            .UpdateSrting("ContinueText", new TableRefHelper.TranslationData() {
                en_US = "CONTINUE?"
            })
            .UpdateSrting("WaitingText", new TableRefHelper.TranslationData() {
                en_US = "WAITING"
            })
            .Build(TableRefHelper.stringTableDefault);

        public static LocalizedString VictoryText { get { return translationTable.GenerateString(TableRefHelper.stringTableDefault, "VictoryText"); } }
        public static LocalizedString RematchText { get { return translationTable.GenerateString(TableRefHelper.stringTableDefault, "RematchText"); } }
        public static LocalizedString ContinueText { get { return translationTable.GenerateString(TableRefHelper.stringTableDefault, "ContinueText"); } }
        public static LocalizedString WaitingText { get { return translationTable.GenerateString(TableRefHelper.stringTableDefault, "WaitingText"); } }

    }
}
