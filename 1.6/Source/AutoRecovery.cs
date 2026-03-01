using System;
using System.Collections.Generic;
using Verse;
using rjw;
namespace SlimeGirl
{
    public class HediffCompProperties_AutoRecovery : HediffCompProperties
    {
        public int tickMultiflier = 6000;
        public float healPoint = 1;
        public HediffCompProperties_AutoRecovery()
        {
            compClass = typeof(HediffComp_AutoRecovery);
        }
    }

    public class HediffComp_AutoRecovery : HediffComp
    {
        public static Dictionary<BodyPartDef, HediffDef> bodyMaps = new() { { xxx.anusDef, Genital_Helper.slime_anus }, { xxx.genitalsDef, Genital_Helper.slime_vagina }, { xxx.breastsDef, Genital_Helper.slime_breasts } };
        public static int count=0;
        private int ticksToHeal;
        private float healPoint;
        //public static Func<Hediff, bool> func;
        public HediffCompProperties_AutoRecovery Props
        {
            get
            {
                return (HediffCompProperties_AutoRecovery)props;
            }
        }

        public override void CompPostMake()
        {
            base.CompPostMake();
            ResetTicksToHeal();
            //var health = Pawn?.health;
            //var hediffs = health?.hediffSet?.hediffs;
            //if (hediffs?.Find((Hediff h) => h.def == Genital_Helper.slime_breasts) == null)
            //{
            //    Pawn.health.AddHediff(Genital_Helper.slime_breasts, Pawn.RaceProps.body.AllParts.Find((BodyPartRecord x) => x.def.defName == "Chest"));
            //}
            //if (hediffs?.Find((Hediff h) => h.def == Genital_Helper.slime_penis) == null)
            //{
            //    return;
            //}
            //health?.RemoveHediff(Pawn.health.hediffSet.hediffs.Find((Hediff h) => h.def == Genital_Helper.slime_penis));

        }

        private void ResetTicksToHeal()
        {
            healPoint = Rand.Range(1f, 3f) * Props.healPoint;
            ticksToHeal = Rand.Range(15, 30) * Props.tickMultiflier;
        }

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            ticksToHeal -= delta;
            if (ticksToHeal <= 0)
            {
                TryHealRandomPermanentWound();
                ResetTicksToHeal();
            }
        }

        private void TryHealRandomPermanentWound()
        {
            List<Hediff> hediffs = Pawn?.health?.hediffSet?.hediffs;
            for (int i = hediffs.Count - 1; i >= 0; i--)
            {
                Hediff h = hediffs[i];
                if (h is Hediff_Injury injury && injury.Severity > 0)
                {
                    injury.Heal(healPoint);
                    return;
                }

                if (h is Hediff_MissingPart missingPart)
                {
                    BodyPartRecord part = missingPart.Part;
                    Pawn.health.RestorePart(part);

                    if (bodyMaps.TryGetValue(part.def, out HediffDef slimePartDef))
                    {
                        Pawn.health.AddHediff(slimePartDef, part);
                    }
                    return;
                }
            }
        }

        public override void CompExposeData()
        {
            Scribe_Values.Look(ref ticksToHeal, "ticksToHeal", 0, false);
        }

        public override string CompDebugString()
        {
            return "ticksToHeal: " + ticksToHeal;
        }

    }
}
