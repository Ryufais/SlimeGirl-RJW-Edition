using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.Sound;
using UnityEngine;
using HarmonyLib;
using System.Reflection.Emit;
using RimWorld.Planet;
using rjw;

namespace SlimeGirl
{
    //[StaticConstructorOnStartup]
    //public static class PatchSet
    //{
    //    static PatchSet()
    //    {
    //        Harmony harmonyInstance = new("com.ColorPatch.rimworld.mod");
    //        harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
    //    }
    //}
    //[HarmonyPatch(typeof(GenRecipe))]
    //[HarmonyPatch(nameof(GenRecipe.PostProcessProduct))]
    //public static class ProductFinishGenColorHook
    //{

    //    [HarmonyPrefix]
    //    static void Prefix(ref Thing product)
    //    {
    //        if (product is ThingWithComps twc)
    //        {
    //            CustomThingDef def = twc.def as CustomThingDef;
    //            if (def != null && !def.followStuffColor)
    //            {
    //                twc.SetColor(Color.white);
    //            }
    //        }
    //    }
    //}
    //[HarmonyPatch(typeof(PawnApparelGenerator))]
    //[HarmonyPatch(nameof(PawnApparelGenerator.GenerateStartingApparelFor))]
    //public static class PawnGenColorHook
    //{
    //    [HarmonyPostfix]
    //    static void Postfix(ref Pawn pawn)
    //    {
    //        if (pawn.apparel != null)
    //        {
    //            List<Apparel> wornApparel = pawn.apparel.WornApparel;
    //            for (int i = 0; i < wornApparel.Count; i++)
    //            {
    //                CustomThingDef def = wornApparel[i].def as CustomThingDef;
    //                if (def != null && !def.followStuffColor)
    //                {
    //                    wornApparel[i].SetColor(Color.white);
    //                    wornApparel[i].SetColor(Color.black);
    //                    wornApparel[i].SetColor(Color.white);
    //                }
    //            }
    //        }
    //    }
    //}
    //[HarmonyPatch(typeof(ThingMaker))]
    //[HarmonyPatch(nameof(ThingMaker.MakeThing))]
    //public static class ThingMakeColorHook
    //{
    //    [HarmonyPostfix]
    //    static void Postfix(ref Thing __result)
    //    {
    //        if (__result is ThingWithComps twc)
    //        {
    //            CustomThingDef def = twc.def as CustomThingDef;
    //            if (def != null && !def.followStuffColor)
    //            {
    //                twc.SetColor(Color.white);
    //                twc.SetColor(Color.black);
    //                twc.SetColor(Color.white);
    //            }
    //        }
    //    }
    //}

    //public class CustomThingDef : ThingDef
    //{
    //    public bool followStuffColor = true;
    //}
    //[HarmonyPatch(typeof(ForbidUtility))]
    //[HarmonyPatch(nameof(ForbidUtility.SetForbidden))]
    //public static class ForbidPatch
    //{

    //    [HarmonyPrefix]
    //    static bool Prefix(this Thing t, bool value, bool warnOnFail = true)
    //    {
    //        if (t == null)
    //        {
    //            if (warnOnFail)
    //            {
    //                ModLog.Error($"Tried to SetForbidden on null Thing.");
    //            }
    //            return false;
    //        }
    //        if (t is not ThingWithComps thingWithComps)
    //        {
    //            if (warnOnFail)
    //            {
    //                ModLog.Error($"Tried to SetForbidden on non-ThingWithComps Thing" + t);
    //            }
    //            return false;
    //        }
    //        CompForbiddable comp = thingWithComps.GetComp<CompForbiddable>();
    //        if (comp == null)
    //        {
    //            return false;
    //        }
    //        comp.Forbidden = value;
    //        return false;
    //    }

    //}
}