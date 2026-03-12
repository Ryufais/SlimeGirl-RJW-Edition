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
    public class SlimeGirlSettingsController : Mod
    {
        public SlimeGirlSettingsController(ModContentPack content) : base(content)
        {
            GetSettings<Settings>();
        }

        public override string SettingsCategory()
        {
            return "SLSettings".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {          
            Settings.DoWindowContents(inRect);
        }
    }
}
