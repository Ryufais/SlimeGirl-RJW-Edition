using LudeonTK;
using RimWorld;
using rjw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SlimeGirl
{
    public static class Debug
    {
        [DebugAction("SlimeGirl", "Update Body", false, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void UpdateBody(Pawn pawn)
        {
            if (pawn?.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Hediff_Slime) is Hediff_Slime slimeHediff)
            {
                int currentLevel = slimeHediff.GetCurrentBodyType();
                SlimeCore.ChangeBodyType(pawn, currentLevel);

                Messages.Message($"Updated {pawn.LabelShort} to body level {currentLevel}", MessageTypeDefOf.TaskCompletion);
            }
        }
    }
}
