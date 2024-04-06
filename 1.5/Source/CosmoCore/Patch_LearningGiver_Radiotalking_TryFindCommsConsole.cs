using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;
using HarmonyLib;

namespace CosmoCore
{
    [HarmonyPatch(typeof(LearningGiver_Radiotalking), "TryFindCommsConsole")]
    public static class Patch_LearningGiver_Radiotalking_TryFindCommsConsole
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn pawn, ref Thing commsConsole, ref bool __result)
        {
            if (commsConsole == null)
            {
                commsConsole = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(CosmoCoreDefOf.CosRim_MacroCommsConsole), PathEndMode.InteractionCell, TraverseParms.For(pawn), 9999f, (Thing t) => t is Building_CommsConsole building_CommsConsole && building_CommsConsole.CanUseCommsNow && pawn.CanReserve(building_CommsConsole) && !building_CommsConsole.IsForbidden(pawn));
                __result = commsConsole != null;
            }
        }
    }
}
