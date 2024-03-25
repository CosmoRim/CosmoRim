using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace CosmoCore
{
    [StaticConstructorOnStartup]
    public static class CosmoCoreStartup
    {
        static CosmoCoreStartup()
        {
            CosmoCoreSettings s = CosmoCoreMod.settings;

            RunAdjustments_Thumper(s);
        }

        public static void RunAdjustments_Thumper(CosmoCoreSettings s)
        {
            CosmoCoreDefOf.CosRim_InsectoidThumper.specialDisplayRadius = s.thumperRange;
        }
    }
}
