using RimWorld;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;


namespace SlimeGirl
{

    [StaticConstructorOnStartup]
    public class SlimeCorpse : Corpse
    {
        public static readonly string CorpsePath = "Things/Slime_Corpse";

        public static readonly Graphic slimeCorpseTexture = GraphicDatabase.Get<Graphic_Single>(path: CorpsePath, shader: ShaderDatabase.Cutout, drawSize: new Vector2(2, 2), color: Color.white);

        private List<BodyPartRecord> _corePartsCache;
        public List<BodyPartRecord> CoreParts
        {
            get
            {
                _corePartsCache ??= InnerPawn?.RaceProps?.body?.GetPartsWithTag(BodyPartTagDefOf.ConsciousnessSource);
                return _corePartsCache;
            }
        }

        public int innerTime;
        public int reviveTime;
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            if (respawningAfterLoad) return;

            int baseReviveTicks = GenTicks.SecondsToTicks(1000);
            Pawn slime = InnerPawn;
            innerTime = baseReviveTicks;
            if (slime != null)
            {
                if (slime.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Hediff_Slime) is Hediff_Slime hediffWithComps)
                {
                    var comp = hediffWithComps.TryGetComp<HediffComp_DeathCounter>();

                    if (comp != null)
                    {
                        comp.deaths++;

                        int deathMultiplier = Mathf.Min(comp.deaths, 10);
                        reviveTime = baseReviveTicks * deathMultiplier;
                    }
                    else
                    {
                        comp = new HediffComp_DeathCounter
                        {
                            parent = hediffWithComps
                        };
                        hediffWithComps.comps.Add(comp);

                        comp.deaths = 1;
                        reviveTime = baseReviveTicks;
                    }

                    innerTime = reviveTime;
                }

            }

            IncidentWorker_SlimeTransportPodCrash.livingSlime = null;
            IncidentWorker_SlimeTransportPodCrash.SlimeCorpse = this;
        }

        public override void TickRare()
        {
            base.TickRare();
            if (Destroyed)
            {
                return;
            }
            if (Bugged)
            {
                ModLog.Error(this + " has null innerPawn. Destroying.");
                Destroy(DestroyMode.Vanish);
                return;
            }
            Pawn pawn = InnerPawn;
            var hediff = pawn?.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Hediff_Slime);
            var comp = hediff?.TryGetComp<HediffComp_DeathCounter>();

            if (comp?.deaths >= Settings.PermaDeathThreshold || IsCoreDestroyed(pawn)) return;

            if (innerTime > 0)
            {
                innerTime += -250;
            }
            else
            {
                Resurrect();
                //innerTime = reviveTime;
            }

        }

        private void Resurrect()
        {
            Pawn innerPawn = InnerPawn;
            ResurrectionUtility.TryResurrect(innerPawn);
            Messages.Message("MessagePawnResurrected".Translate(innerPawn).CapitalizeFirst(), innerPawn, MessageTypeDefOf.PositiveEvent, true);
            IncidentWorker_SlimeTransportPodCrash.livingSlime = InnerPawn;
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref innerTime, "innerTime");
        }
        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            IncidentWorker_SlimeTransportPodCrash.SlimeCorpse = null;
            base.Destroy(mode);
        }
        public override string GetInspectString()
        {
            base.GetInspectString();
            StringBuilder stringBuilder = new();
            Pawn pawn = InnerPawn;
            if (pawn.Faction != null)
            {
                stringBuilder.AppendLine("Faction".Translate() + ": " + pawn.Faction.Name);
            }

            var hediff = pawn?.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Hediff_Slime);
            var comp = hediff?.TryGetComp<HediffComp_DeathCounter>();

            stringBuilder.AppendLine("DeadTime".Translate(Age.ToStringTicksToPeriodVague(true, false)));

            if (comp?.deaths >= Settings.PermaDeathThreshold || IsCoreDestroyed(pawn))
            {
                stringBuilder.AppendLine("SlimeCoreDead".Translate());
            }
            else
            {
                stringBuilder.AppendLine("SLRemainTime".Translate() + ": " + innerTime.ToStringTicksToPeriod());
            }


            //if (num != 0f)
            //{
            //    stringBuilder.AppendLine("RemainTime".Translate() + ": " + innerTime.ToStringTicksToPeriod());
            //}
            return stringBuilder.ToString().TrimEndNewlines();
        }
        public override void DynamicDrawPhaseAt(DrawPhase draw, Vector3 drawLoc, bool flip = false)
        {
            slimeCorpseTexture.Draw(drawLoc, Rot4.North, this, 0f);
        }

        public bool IsCoreDestroyed(Pawn pawn)
        {
            var parts = CoreParts;
            if (parts == null) return false;

            for (int i = 0; i < parts.Count; i++)
            {
                if (pawn.health.hediffSet.PartIsMissing(parts[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }

}
