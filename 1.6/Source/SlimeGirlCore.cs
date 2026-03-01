using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using rjw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Noise;
using Verse.Sound;


namespace SlimeGirl
{
    [StaticConstructorOnStartup]
    public static class SlimeGirlRJWPatch
    {
        private static readonly Type patchType = typeof(SlimeGirlRJWPatch);
        public static BodyTypeDef[] SlimeBodyLookup = new BodyTypeDef[34]; 
         
        static SlimeGirlRJWPatch()
        {
            Harmony harmonyInstance = new("com.SlimeGirl.rimworld.mod");
            harmonyInstance.Patch(AccessTools.Method(typeof(SexUtility), nameof(SexUtility.ProcessSex)), null, new HarmonyMethod(patchType, nameof(ProcessSexPostFix)), null);
            //harmonyInstance.Patch(AccessTools.Method(typeof(Pawn), nameof(Pawn.MakeCorpse),
            //[typeof(Building_Grave), typeof(bool), typeof(float)]),
            // new HarmonyMethod(patchType, nameof(MakeCorpsePrefix)), null, null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Pawn_NeedsTracker), nameof(Pawn_NeedsTracker.ShouldHaveNeed)), null, new HarmonyMethod(patchType, nameof(ShouldHaveNeedPostfix)), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(WildManUtility), nameof(WildManUtility.IsWildMan)), null, new HarmonyMethod(patchType, nameof(IsWildMan_Postfix)), null);

            SlimeRaceDefOf.Corpse_Rjw_Slime_Blue?.thingClass = typeof(SlimeCorpse);
         
            SlimeBodyLookup[0] = SlimeBodyTypeDefOf.AA;
            SlimeBodyLookup[1] = SlimeBodyTypeDefOf.AB;
            SlimeBodyLookup[2] = SlimeBodyTypeDefOf.AC;
            SlimeBodyLookup[3] = SlimeBodyTypeDefOf.AD;

            SlimeBodyLookup[10] = SlimeBodyTypeDefOf.BA;
            SlimeBodyLookup[11] = SlimeBodyTypeDefOf.BB;
            SlimeBodyLookup[12] = SlimeBodyTypeDefOf.BC;
            SlimeBodyLookup[13] = SlimeBodyTypeDefOf.BD;

            SlimeBodyLookup[20] = SlimeBodyTypeDefOf.CA;
            SlimeBodyLookup[21] = SlimeBodyTypeDefOf.CB;
            SlimeBodyLookup[22] = SlimeBodyTypeDefOf.CC;
            SlimeBodyLookup[23] = SlimeBodyTypeDefOf.CD;

            SlimeBodyLookup[30] = SlimeBodyTypeDefOf.DA;
            SlimeBodyLookup[31] = SlimeBodyTypeDefOf.DB;
            SlimeBodyLookup[32] = SlimeBodyTypeDefOf.DC;
            SlimeBodyLookup[33] = SlimeBodyTypeDefOf.DD;
        }
        public static void ProcessSexPostFix(SexProps props)
        {
            Pawn pawn = props?.pawn;
            Pawn partner = props?.partner;

            if (partner != null && partner.def == SlimeRaceDefOf.Rjw_Slime_Blue)
            {
                if (partner.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Hediff_Slime) is Hediff_Slime hediff_Slime)
                {
                    hediff_Slime.Severity += Mathf.Clamp((pawn.BodySize / 3f), 0, 1);//00~30
                    hediff_Slime.count = 0;
                    hediff_Slime.severityIsZero = false;
                    SlimeCore.ChangeBodyType(partner, hediff_Slime.GetCurrentBodyType());
                }
            }

            if (pawn != null && pawn.def == SlimeRaceDefOf.Rjw_Slime_Blue)
            {
                if (pawn.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Hediff_Slime) is Hediff_Slime hediff_Slime)
                {
                    hediff_Slime.Severity += Mathf.Clamp((partner.BodySize / 2.2f), 0, 1);//00~30
                    hediff_Slime.count = 0;
                    hediff_Slime.severityIsZero = false;
                    SlimeCore.ChangeBodyType(pawn, hediff_Slime.GetCurrentBodyType());
                }
            }
        }

        public static void IsWildMan_Postfix(Pawn p, ref bool __result)
        {
            if (__result) return;

            if (p.kindDef == SlimePawnKindDefOf.Slime_Wild && p.Faction == null)
            {
                __result = true;
            }
        }

        //public static void ShouldBeDeadFromLethalDamageThresholdPostFix(Pawn_HealthTracker __instance,ref bool __result, Pawn ___pawn)
        //{
        //    float num = 0f;
        //    for (int i = 0; i < __instance.hediffSet.hediffs.Count; i++)
        //    {
        //        if (__instance.hediffSet.hediffs[i] is Hediff_Injury)
        //        {
        //            num += __instance.hediffSet.hediffs[i].Severity;
        //        }
        //    }
        //    //Log.Message(___pawn + " Lethal Damage Threshold: " + __instance.LethalDamageThreshold + " <="+ num+" // result: " + __result);
        //}
        //public static void ShouldBeDeadPostFix(Pawn_HealthTracker __instance, ref bool __result, Pawn ___pawn)
        //{
        //    //Log.Message(___pawn + " ShouldBeDeadPostFix: "+ __result);
        //}
        //public static bool MakeCorpsePrefix(Pawn __instance,ref Corpse __result, Building_Grave assignedGrave, bool inBed, float bedRotation)
        //{
        //    if (__instance.holdingOwner != null)
        //    {
        //        ModLog.Warning("We can't make corpse because the pawn is in a ThingOwner. Remove him from the container first. This should have been already handled before calling this method. holder=" + __instance);
        //        __result = null;
        //        return false;
        //    }
        //    Corpse corpse;
        //    if (__instance.def == SlimeRaceDefOf.Rjw_Slime_Blue)
        //    {
        //        //Log.Message(__instance.RaceProps.corpseDef.ToString());
        //        corpse = ThingMaker.MakeThing(__instance.RaceProps.corpseDef, null) as SlimeCorpse;
        //        if (corpse == null)
        //        {
        //            Log.Message("corpse is null");
        //        }
        //        //Log.Message("SlimeCorpse");
        //    }
        //    else
        //    {
        //        corpse = (Corpse)ThingMaker.MakeThing(__instance.RaceProps.corpseDef, null);
        //    }
        //    corpse.InnerPawn = __instance;
        //    if (assignedGrave != null)
        //    {
        //        corpse.InnerPawn.ownership.ClaimGrave(assignedGrave);
        //    }
        //    if (inBed)
        //    {
        //        corpse.InnerPawn.Drawer.renderer.wiggler.SetToCustomRotation(bedRotation + 180f);
        //    }
        //    __result = corpse;
        //    return false;
        //}
        //public static void ReduceDamageToPreserveOutsidePartsPreFix(ref float __result, ref float postArmorDamage, ref DamageInfo dinfo, Pawn pawn)
        //{
        //    //Log.Message("postArmorDamage: " + postArmorDamage);
        //}
        //public static void ReduceDamageToPreserveOutsidePartsPostFix(ref float __result,ref float postArmorDamage, ref DamageInfo dinfo, Pawn pawn)
        //{
        //    if(pawn.def == SlimeRaceDefOf.Slime && pawn.health.hediffSet.GetPartHealth(dinfo.HitPart)<=postArmorDamage)
        //    {
        //        __result = 1;
        //    }
        //}
        //public static bool ImpregnatePreFix(Pawn pawn, Pawn partner)
        //{
        //    if(pawn.def == SlimeRaceDefOf.Slime || partner.def == SlimeRaceDefOf.Slime)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        /*  
          public static bool calculateAndApplySemenPrefix(Pawn giver, Pawn receiver, SexProps props)
          {
             // if (!RJWSettings.cum_on_body) return false;

              Pawn pawn2;
              Pawn pawn3;
              if (Genital_Helper.has_penis_fertile(giver) || Genital_Helper.has_penis_infertile(giver) || xxx.is_mechanoid(giver) || xxx.is_insect(giver))
              {
                  pawn2 = giver;
                  pawn3 = receiver;
              }
              else
              {
                  if (receiver == null || (!(Genital_Helper.has_penis_fertile(receiver) || Genital_Helper.has_penis_infertile(receiver)) && !xxx.is_mechanoid(receiver) && !xxx.is_insect(receiver)))
                  {
                      return false;
                  }
                  pawn2 = receiver;
                  pawn3 = giver;
              }
              int semenType = 0;
              if (xxx.is_mechanoid(pawn2))
              {
                  semenType = 2;
              }
              else if (xxx.is_insect(pawn2))
              {
                  semenType = 1;
              }
              BodyPartRecord bodyPartRecord;
              if (xxx.is_mechanoid(pawn2))
              {
                  bodyPartRecord = pawn2.RaceProps.body.AllParts.Find((BodyPartRecord x) => string.Equals(x.def.defName, "MechGenitals"));
              }
              else
              {
                  bodyPartRecord = pawn2.RaceProps.body.AllParts.Find((BodyPartRecord x) => x.def == xxx.genitalsDef);
              }
              if (bodyPartRecord == null)
              {
                  return false;
              }
              Need need = pawn2.needs.AllNeeds.Find((Need x) => string.Equals(x.def.defName, "Sex"));
              float num = 1f;
              if (need != null)
              {
                  num = 1f - need.CurLevel;
              }
              float num2 = Math.Min((80 / SexUtility.ScaleToHumanAge(pawn2, 80)), 1f);
              float num3 = num * pawn2.BodySize * num2 * RJWSettings.cum_on_body_amount_adjust;
              if (xxx.has_quirk(pawn2, "Messy"))
              {
                  num3 *= 1.5f;
              }
              if (partner == null && sextype == xxx.rjwSextype.Masturbation)
              {
                  if (!xxx.is_slime(pawn2))
                  {
                      SemenHelper.cumOn(pawn2, bodyPartRecord, num3 * 0.3f, pawn2, 0);
                  }
                  return false;
              }
              if (partner != null)
              {
                  List<BodyPartRecord> list = new List<BodyPartRecord>();
                  IEnumerable<BodyPartRecord> availableBodyParts = SemenHelper.getAvailableBodyParts(pawn3);
                  switch (sextype)
                  {
                      case xxx.rjwSextype.None:
                          num3 = 0f;
                          break;
                      case xxx.rjwSextype.Vaginal:
                          list.Add(pawn3.RaceProps.body.AllParts.Find((BodyPartRecord x) => x.def == SemenHelper.genitalsDef));
                          break;
                      case xxx.rjwSextype.Anal:
                          list.Add(pawn3.RaceProps.body.AllParts.Find((BodyPartRecord x) => x.def == SemenHelper.anusDef));
                          break;
                      case xxx.rjwSextype.Oral:
                          list.Add(pawn3.RaceProps.body.AllParts.Find((BodyPartRecord x) => x.def == BodyPartDefOf.mouth));
                          break;
                      case xxx.rjwSextype.Masturbation:
                          num3 *= 2f;
                          break;
                      case xxx.rjwSextype.DoublePenetration:
                          list.Add(pawn3.RaceProps.body.AllParts.Find((BodyPartRecord x) => x.def == SemenHelper.anusDef));
                          list.Add(pawn3.RaceProps.body.AllParts.Find((BodyPartRecord x) => x.def == SemenHelper.genitalsDef));
                          break;
                      case xxx.rjwSextype.Boobjob:
                          list.Add(pawn3.RaceProps.body.AllParts.Find((BodyPartRecord x) => x.def == SemenHelper.chestDef));
                          break;
                      case xxx.rjwSextype.Handjob:
                          {
                              BodyPartRecord item;
                              availableBodyParts.TryRandomElement(out item);
                              list.Add(item);
                              break;
                          }
                      case xxx.rjwSextype.Footjob:
                          {
                              BodyPartRecord item;
                              availableBodyParts.TryRandomElement(out item);
                              list.Add(item);
                              break;
                          }
                      case xxx.rjwSextype.Fingering:
                          num3 = 0f;
                          break;
                      case xxx.rjwSextype.Scissoring:
                          list.Add(pawn3.RaceProps.body.AllParts.Find((BodyPartRecord x) => x.def == SemenHelper.genitalsDef));
                          break;
                      case xxx.rjwSextype.MutualMasturbation:
                          {
                              BodyPartRecord item;
                              availableBodyParts.TryRandomElement(out item);
                              list.Add(item);
                              break;
                          }
                      case xxx.rjwSextype.Fisting:
                          num3 = 0f;
                          break;
                      case xxx.rjwSextype.MechImplant:
                          {
                              int num4 = Rand.Range(0, 3);
                              if (num4 == 0)
                              {
                                  list.Add(pawn3.RaceProps.body.AllParts.Find((BodyPartRecord x) => x.def == SemenHelper.genitalsDef));
                              }
                              else if (num4 == 1)
                              {
                                  list.Add(pawn3.RaceProps.body.AllParts.Find((BodyPartRecord x) => x.def == SemenHelper.anusDef));
                              }
                              else if (num4 == 2)
                              {
                                  list.Add(pawn3.RaceProps.body.AllParts.Find((BodyPartRecord x) => x.def == BodyPartDefOf.Jaw));
                              }
                              break;
                          }
                  }
                  if (num3 > 0f)
                  {
                      if (xxx.is_slime(pawn3))
                      {

                      }
                      SemenHelper.cumOn(pawn2, bodyPartRecord, num3 * 0.3f, pawn2, semenType);
                      foreach (BodyPartRecord bodyPartRecord2 in list)
                      {
                          if (bodyPartRecord2 != null)
                          {
                              SemenHelper.cumOn(pawn3, bodyPartRecord2, num3, pawn2, semenType);
                          }
                      }
                  }
              }
              return false;
          }
        */
        public static void ShouldHaveNeedPostfix(ref bool __result, NeedDef nd, Pawn ___pawn)
        {
            if (___pawn.def == SlimeRaceDefOf.Rjw_Slime_Blue)
            {
                if(nd == SlimeNeedDefOf.Outdoors)
                {
                    __result = false;
                }
            }
        }
        
    }
    public class Hediff_Slime : Hediff
    {
        public int bellyLevel;
        //public Hediff bukake;

        public bool severityIsZero;

        public int maxCount;
        public float tickPerHunger;

        public int count;
        #region Properties
        public override bool ShouldRemove => false;
        #endregion

        public override float Severity
        {
            get
            {
                return severityInt;
            }
            set
            {
                base.Severity = value;

            }
        }
        public override void PostMake()
        {
            base.PostMake();
            maxCount = ((Hediff_SlimeDef)def).maxCount;
            tickPerHunger = ((Hediff_SlimeDef)def).tickPerHunger;
            SlimeCore.ChangeBodyType(pawn, GetCurrentBodyType());
            //Log.Message("PostMake - bellyLevel: " + bellyLevel + " severity: " + severityInt);
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);          
            SlimeCore.ChangeBodyType(pawn, GetCurrentBodyType());
        }

        public override void TickInterval(int delta)
        {
            if (!severityIsZero)
            {
                count += delta;

                if (count >= maxCount)
                {
                    count -= maxCount;

                    if (severityInt > 0)
                    {
                        if(CurStageIndex > 0)
                        {
                            bellyLevel = Mathf.Min(bellyLevel + 1, 3);
                            //Log.Message("" + bellyLevel * 10 + CurStageIndex);
                        }
                    }

                    else
                    {
                        if (bellyLevel > 0)
                        {
                            bellyLevel--;
                        }
                        else
                        {
                            severityIsZero = true;
                        }
                    }
                    SlimeCore.ChangeBodyType(pawn, GetCurrentBodyType());
                }
                
                float hungerFactor = tickPerHunger * delta;

                Severity -= hungerFactor;

                pawn?.needs?.food?.CurLevel += hungerFactor * 10;
            }
            //Log.Message("Tick");
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
        }
        public override void ExposeData()
        {
            base.ExposeData();
            maxCount = ((Hediff_SlimeDef)def).maxCount;
            tickPerHunger = ((Hediff_SlimeDef)def).tickPerHunger;
            Scribe_Values.Look(ref count, "count");
            Scribe_Values.Look(ref bellyLevel, "bellyLevel");
            Scribe_Values.Look(ref severityIsZero, "dirtyFlag");
        }
        public int GetCurrentBodyType()
        {
            return bellyLevel*10 + CurStageIndex;
        }
        public override string TipStringExtra
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("CurStageIndex: " + CurStageIndex +
                    "\ncount: " + count +
                    "\nbellyLevel: " + bellyLevel +
                    "\nseverityIsZero: " + severityIsZero);
                return stringBuilder.ToString();
            }
        }

    }

    public class Hediff_SlimeDef:HediffDef
    {
        public int maxCount;
        public float tickPerHunger;
        
    }

    public static class SlimeCore
    {
        public static void ChangeBodyType(Pawn pawn, int level)
        {
            BodyTypeDef currentBodyType = pawn?.story?.bodyType;

            int searchIndex = Mathf.Clamp(level, 0, SlimeGirlRJWPatch.SlimeBodyLookup.Length - 1);

            BodyTypeDef chosenBodyType = null;

            for (int i = searchIndex; i >= 0; i--)
            {
                if (SlimeGirlRJWPatch.SlimeBodyLookup[i] != null)
                {
                    chosenBodyType = SlimeGirlRJWPatch.SlimeBodyLookup[i];
                    break; 
                }
            }

            if (chosenBodyType != null && currentBodyType != chosenBodyType)
            {
                pawn.story?.bodyType = chosenBodyType;

                pawn.Drawer.renderer.SetAllGraphicsDirty();
                PortraitsCache.SetDirty(pawn);
            }
            
            /*
            switch (level)
            {
                case 00:
                    pawn.story.bodyType = SlimeBodyDefOf.AA;
                    break;
                case 01:
                    pawn.story.bodyType = SlimeBodyDefOf.AB;
                    break;
                case 02:
                    pawn.story.bodyType = SlimeBodyDefOf.AC;
                    break;
                case 03:
                    pawn.story.bodyType = SlimeBodyDefOf.AD;
                    break;
                case 10:
                    pawn.story.bodyType = SlimeBodyDefOf.BA;
                    break;
                case 11:
                    pawn.story.bodyType = SlimeBodyDefOf.BB;
                    break;
                case 12:
                    pawn.story.bodyType = SlimeBodyDefOf.BC;
                    break;
                case 13:
                    pawn.story.bodyType = SlimeBodyDefOf.BD;
                    break;
                case 20:
                    pawn.story.bodyType = SlimeBodyDefOf.CA;
                    break;
                case 21:
                    pawn.story.bodyType = SlimeBodyDefOf.CB;
                    break;
                case 22:
                    pawn.story.bodyType = SlimeBodyDefOf.CC;
                    break;
                case 23:
                    pawn.story.bodyType = SlimeBodyDefOf.CD;
                    break;
                case 30:
                    pawn.story.bodyType = SlimeBodyDefOf.DA;
                    break;
                case 31:
                    pawn.story.bodyType = SlimeBodyDefOf.DB;
                    break;
                case 32:
                    pawn.story.bodyType = SlimeBodyDefOf.DC;
                    break;
                case 33:
                    pawn.story.bodyType = SlimeBodyDefOf.DD;
                    break;
                default:
                    break;
            }*/          
        }
    }

}
