using rjw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace SlimeGirl
{
    public class Settings : ModSettings
    {
        public static int PermaDeathThreshold = 11;

        private static Vector2 scrollPosition;
        private static float height_modifier = 0f;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref PermaDeathThreshold, "PermaDeathThreshold", 11);
        }
        public static void DoWindowContents(Rect inRect)
        {
            Rect outRect = new(0f, 30f, inRect.width, inRect.height - 30f);
            Rect viewRect = new(0f, 30f, inRect.width - 16f, inRect.height + height_modifier);

            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);

            Listing_Standard listingStandard = new()
            {
                maxOneColumn = true,
                ColumnWidth = viewRect.width / 2.05f
            };

            listingStandard.Begin(viewRect);
            listingStandard.Gap(12f);

            using (UiScope.title)
            {
                listingStandard.Label("SLSettingsCorpseTitle".Translate());
            }

            listingStandard.Label("SLPermaDeathThreshold".Translate(PermaDeathThreshold), -1f, new TipSignal("SLPermaDeathThreshold_desc".Translate()));
            PermaDeathThreshold = (int)listingStandard.Slider(PermaDeathThreshold, 11, 20);

            listingStandard.End();
            height_modifier = Math.Max(0, listingStandard.MaxColumnHeightSeen - inRect.height);
            Widgets.EndScrollView();
        }
    }
}
