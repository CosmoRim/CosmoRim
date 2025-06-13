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
    
    [DefOf]
    public static class CosmoCoreDefOf
    {
        public static ThingDef 
            CosRim_InsectoidThumper,
            CosRim_MacroCommsConsole,
            CosRim_VRTerminal,
            CosRim_VRProgrammingBench;

        public static ThingDef 
            CosRim_VRChip_Blank;

        public static SoundDef 
            CosRim_Sound_ThumperHit,
            CosRim_Sound_ThumperStartup,
            CosRim_Sound_ThumperShutdown;

        public static ThingCategoryDef 
            CosRim_VRChips;

        public static ResearchProjectDef 
            CosRim_VRGuideChips;
    }
}
