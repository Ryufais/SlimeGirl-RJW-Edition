using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace SlimeGirl
{
    public class IncidentWorker_SlimeWandersIn : IncidentWorker
    {

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            return base.CanFireNowSub(parms) && map.mapTemperature.SeasonAcceptableFor(SlimeRaceDefOf.Rjw_Slime_Blue);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            if (!RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 cell, map, CellFinder.EdgeRoadChance_Ignore)) return false;

            PawnKindDef slimeKind = SlimePawnKindDefOf.Slime_Wild;
            Pawn pawn = PawnGenerator.GeneratePawn(slimeKind, null);

            GenSpawn.Spawn(pawn, cell, map);

            TaggedString label = def.letterLabel.Formatted(pawn.Named("PAWN"));
            TaggedString text = def.letterText.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn);
            SendStandardLetter(label, text, def.letterDef, parms, pawn);

            return true;
        }
    }
}
