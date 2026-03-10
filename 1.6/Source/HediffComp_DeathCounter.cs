using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SlimeGirl
{
    public class HediffCompProperties_DeathCounter : HediffCompProperties
    {
        public HediffCompProperties_DeathCounter()
        {
            this.compClass = typeof(HediffComp_DeathCounter);
        }
    }

    public class HediffComp_DeathCounter : HediffComp
    {
        public int deaths = 0;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref deaths, "deaths", 0);
        }
    }
}
