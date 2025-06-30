using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using Verse;
using TabulaRasa;

namespace CosmoCore
{
    public class CosmoCoreMod : Mod
    {
        public static CosmoCoreMod mod;
        public static CosmoCoreSettings settings;

        public Vector2 optionsScrollPosition;
        public float optionsViewRectHeight;
        public Dictionary<string, bool> collapsedCategories = new Dictionary<string, bool>();

        internal static string VersionDir => Path.Combine(mod.Content.ModMetaData.RootDir.FullName, "Version.txt");
        public static string CurrentVersion { get; private set; }

        public CosmoCoreMod(ModContentPack content) : base(content)
        {
            mod = this;
            settings = GetSettings<CosmoCoreSettings>();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            CurrentVersion = $"{version.Major}.{version.Minor}.{version.Build}";

            Log.Message($":: CosmoRim :: ".Colorize(Color.cyan) + $"{CurrentVersion} ::");

            if (Prefs.DevMode)
            {
                File.WriteAllText(VersionDir, CurrentVersion);
            }

            collapsedCategories.Clear();
            collapsedCategories.Add("Cat_Thumper", false);
            collapsedCategories.Add("Cat_MacroComms", false);

            Harmony harmony = new Harmony("Neronix17.CosmoCore.RimWorld");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public override string SettingsCategory() => "CosmoRim";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            bool flag = optionsViewRectHeight > inRect.height;
            Rect viewRect = new Rect(inRect.x, inRect.y, inRect.width - (flag ? 26f : 0f), optionsViewRectHeight);
            Widgets.BeginScrollView(inRect, ref optionsScrollPosition, viewRect);
            Listing_Standard listing = new Listing_Standard();
            Rect rect = new Rect(viewRect.x, viewRect.y, viewRect.width, 999999f);
            listing.Begin(rect);
            // ============================ CONTENTS ================================
            DoOptionsCategoryContents(listing);
            // ======================================================================
            optionsViewRectHeight = listing.CurHeight;
            listing.End();
            Widgets.EndScrollView();
        }

        public void DoOptionsCategoryContents(Listing_Standard listing)
        {
            listing.Note("You will need to restart the game for some settings to take effect.", GameFont.Tiny);
            listing.GapLine();
            DoOptions_Defaults(listing);
            DoOptions_Thumper(listing);
            DoOptions_OrbitalBeacon(listing);
        }

        public void DoOptions_Defaults(Listing_Standard listing)
        {
            if (listing.ButtonTextLabeled("Default Settings", "Reset"))
            {
                // Thumper
                settings.soundEnabled = true;
                settings.thumperRange = 24f;
                // Macro Comms
                settings.powerCostOverride = 2;
                settings.pingSound = false;
                settings.ambientSound = true;
            }
        }

        public void DoOptions_Thumper(Listing_Standard listing)
        {
            listing.GapLine();
            bool collapsed = collapsedCategories["Cat_Thumper"];
            listing.LabelBackedHeader("Insectoid Thumper", Color.white, ref collapsed);
            collapsedCategories["Cat_Thumper"] = collapsed;
            if (!collapsedCategories["Cat_Thumper"])
            {
                listing.CheckboxLabeled("Thumper Sound Effect", ref settings.soundEnabled, "Whether or not the regularly repeating 'Thump' is played. Could understandably be annoying especially for those who play on high speeds.");
                listing.AddLabeledSlider($"Range: {settings.thumperRange}", ref settings.thumperRange, 6f, 45f, "Min: 6.0", "Max: 45.0", 0.1f);
                CosmoCoreStartup.RunAdjustments_Thumper(settings);
            }
        }

        public void DoOptions_OrbitalBeacon(Listing_Standard listing)
        {
            listing.GapLine();
            bool collapsed = collapsedCategories["Cat_MacroComms"];
            listing.LabelBackedHeader("Macro Comms Console", Color.white, ref collapsed);
            collapsedCategories["Cat_MacroComms"] = collapsed;
            if (!collapsedCategories["Cat_MacroComms"])
            {
                listing.Label("Energy Cost Per Home Tile:");
                listing.Label("Min: 0, Max: 10, Current: " + settings.powerCostOverride.ToString());
                settings.powerCostOverride = (int)listing.Slider(settings.powerCostOverride, 0, 10);
                listing.CheckboxLabeled("Beacon Ping Sound", ref settings.pingSound, "Enable/Disable ping sound effect, plays occasionally, but could undestandably be annoying for some people.");
                listing.CheckboxLabeled("Beacon Ambient Sound", ref settings.ambientSound, "Enable/Disable ambient sound effect, plays constantly while powered.");
            }
        }
    }
}
