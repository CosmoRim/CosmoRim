using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

using HarmonyLib;

namespace CosmoCore
{
    [HarmonyPatch(typeof(DefGenerator), "GenerateImpliedDefs_PreResolve")]
    public static class Patch_DefGenerator_GenerateImpliedDefs_PreResolve
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            foreach (ThingDef item in ImpliedVRChipDefs())
            {
                DefGenerator.AddImpliedDef(item);
            }
            foreach (RecipeDef item in ImpliedVRRecipes())
            {
                DefGenerator.AddImpliedDef(item);
            }
        }

        public static IEnumerable<ThingDef> ImpliedVRChipDefs()
        {
            foreach(SkillDef skill in DefDatabase<SkillDef>.AllDefs)
            {
                yield return CreateSkillVRChip(skill);
            }
            yield break;
        }

        public static ThingDef CreateSkillVRChip(SkillDef skill)
        {
            ThingDef thing = new ThingDef
            {
                defName = "CosRim_VRChip_" + skill.defName,
                label = "CosmoRim.VRChipLabel".Translate(skill.label),
                description = "CosmoRim.VRChipDescription".Translate(skill.label),
                thingClass = typeof(ThingWithComps),
                category = ThingCategory.Item,
                drawerType = DrawerType.MapMeshOnly,
                resourceReadoutPriority = ResourceCountPriority.Middle,
                useHitPoints = true,
                selectable = true,
                altitudeLayer = AltitudeLayer.Item,
                alwaysHaulable = true,
                drawGUIOverlay = true,
                rotatable = false,
                pathCost = 14,
                graphicData = new GraphicData
                {
                    texPath = "CosmoRim/Items/VRChip/VRChip_a",
                    graphicClass = typeof(Graphic_Single)
                },
                soundInteract = SoundDefOf.Standard_Pickup,
                soundDrop = SoundDefOf.Standard_Drop,
                stackLimit = 25,
                healthAffectsPrice = false,
                statBases = new List<StatModifier>
                {
                    new StatModifier()
                    {
                        stat = StatDefOf.MaxHitPoints,
                        value = 20f
                    },
                    new StatModifier()
                    {
                        stat = StatDefOf.MarketValue,
                        value = 450f
                    },
                    new StatModifier()
                    {
                        stat = StatDefOf.Mass,
                        value = 0.5f
                    },
                    new StatModifier()
                    {
                        stat = StatDefOf.Flammability,
                        value = 0.3f
                    },
                    new StatModifier()
                    {
                        stat = StatDefOf.DeteriorationRate,
                        value = 2f
                    },
                    new StatModifier()
                    {
                        stat = StatDefOf.Beauty,
                        value = -4f
                    }
                },
                comps = new List<CompProperties>
                {
                    new CompProperties_Forbiddable()
                },
                intricate = true,
                thingCategories = new List<ThingCategoryDef>
                {
                    CosmoCoreDefOf.CosRim_VRChips
                },
                allowedArchonexusCount = 10
            };
            return thing;
        }

        public static IEnumerable<RecipeDef> ImpliedVRRecipes()
        {
            foreach (SkillDef skill in DefDatabase<SkillDef>.AllDefs)
            {
                yield return CreateVRSkillRecipe(skill);
                yield return CreateVRChipWriteRecipe(skill);
                yield return CreateVRChipReadRecipe(skill);
            }
            yield break;
        }

        public static RecipeDef CreateVRSkillRecipe(SkillDef skill)
        {
            RecipeDef recipe = new RecipeDef
            {
                defName = "CosRim_VRSkillTraining_" + skill.defName,
                label = "CosmoRim.VRSkillRecipeLabel".Translate(skill.label),
                description = "CosmoRim.VRSkillRecipeDesc".Translate(skill.label),
                jobString = "CosmoRim.VRSkillRecipeJobString".Translate(skill.label),
                workAmount = 2500,
                workSkill = skill,
                workSkillLearnFactor = 2f,
                effectWorking = EffecterDefOf.Hacking,
                soundWorking = SoundDefOf.Hacking_InProgress,
                recipeUsers = new List<ThingDef>
                {
                    CosmoCoreDefOf.CosRim_VRTerminal
                }
            };
            return recipe;
        }

        public static RecipeDef CreateVRChipWriteRecipe(SkillDef skill)
        {
            ThingDef chipDef = DefDatabase<ThingDef>.GetNamed("CosRim_VRChip_" + skill.defName);
            RecipeDef recipe = new RecipeDef
            {
                defName = "CosRim_VRChipWriting_" + skill.defName,
                label = "CosmoRim.VRChipWritingLabel".Translate(skill.label),
                description = "CosmoRim.VRChipWritingDesc".Translate(skill.label),
                jobString = "CosmoRim.VRChipWritingJobString".Translate(skill.label),
                workAmount = 2500,
                workSkill = skill,
                workSkillLearnFactor = 2f,
                effectWorking = EffecterDefOf.Hacking,
                soundWorking = SoundDefOf.Hacking_InProgress,
                recipeUsers = new List<ThingDef>
                {
                    CosmoCoreDefOf.CosRim_VRProgrammingBench
                },
                ingredients = new List<IngredientCount>
                {
                    new IngredientCount
                    {
                        filter = new ThingFilter { thingDefs = new List<ThingDef> { CosmoCoreDefOf.CosRim_VRChip_Blank } },
                        count = 1
                    }
                },
                fixedIngredientFilter = new ThingFilter { thingDefs = new List<ThingDef> { CosmoCoreDefOf.CosRim_VRChip_Blank } },
                products = new List<ThingDefCountClass> { new ThingDefCountClass { thingDef = chipDef, count = 1 } },
                skillRequirements = new List<SkillRequirement>
                {
                    new SkillRequirement
                    {
                        skill = skill,
                        minLevel = 20
                    }
                }
            };
            return recipe;
        }

        public static RecipeDef CreateVRChipReadRecipe(SkillDef skill)
        {
            ThingDef chipDef = DefDatabase<ThingDef>.GetNamed("CosRim_VRChip_" + skill.defName);
            RecipeDef recipe = new RecipeDef
            {
                defName = "CosRim_VRChipTraining_" + skill.defName,
                label = "CosmoRim.VRChipTrainingLabel".Translate(skill.label),
                description = "CosmoRim.VRChipTrainingDesc".Translate(skill.label),
                jobString = "CosmoRim.VRChipTrainingJobString".Translate(skill.label),
                workAmount = 10000,
                workSkill = skill,
                workSkillLearnFactor = 10f,
                effectWorking = EffecterDefOf.Hacking,
                soundWorking = SoundDefOf.Hacking_InProgress,
                recipeUsers = new List<ThingDef>
                {
                    CosmoCoreDefOf.CosRim_VRTerminal
                },
                ingredients = new List<IngredientCount>
                {
                    new IngredientCount
                    {
                        filter = new ThingFilter { thingDefs = new List<ThingDef>{ chipDef } },
                        count = 1
                    }
                },
                fixedIngredientFilter = new ThingFilter { thingDefs = new List<ThingDef> { chipDef } },
                products = new List<ThingDefCountClass> { new ThingDefCountClass { thingDef = CosmoCoreDefOf.CosRim_VRChip_Blank, count = 1 } }
            };
            return recipe;
        }
    }
}
