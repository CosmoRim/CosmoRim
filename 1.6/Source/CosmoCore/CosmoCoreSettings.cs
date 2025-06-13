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
    public class CosmoCoreSettings : ModSettings
    {
        public bool verboseLogging = false;

        // Insectoid Thumper
        public bool soundEnabled = true;
        public float thumperRange = 24f;
        // Orbital Beacon
        public int powerCostOverride = 2;
        public bool pingSound = false;
        public bool ambientSound = true;

        public override void ExposeData()
        {
            base.ExposeData();

            // Insectoid Thumper
            Scribe_Values.Look<bool>(ref soundEnabled, "soundEnabled", true);
            Scribe_Values.Look<float>(ref thumperRange, "thumperRange", 24f);
            // Orbital Beacon
            Scribe_Values.Look<int>(ref powerCostOverride, "powerCostOverride", 2);
            Scribe_Values.Look<bool>(ref pingSound, "pingSound", false);
            Scribe_Values.Look<bool>(ref ambientSound, "ambientSound", true);
        }
    }
}
