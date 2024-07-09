// <copyright file="SnooperMod.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace Snooper
{
    using System.Reflection;
    using ICities;
    using Snooper.Panels;
    using Snooper.Patches;
    using Snooper.Utils;

    /// <summary>
    /// The main class of the Snooper mod.
    /// </summary>
    public sealed class SnooperMod : LoadingExtensionBase, IUserMod
    {
        private const string HarmonyId = "com.cities_skylines.dymanoid.snooper";

        private string modVersion
        {
            get
            {
                var assemblyVersion = ModAssembly.GetName().Version;
                return $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}";
            }
        }

        /// <summary>
        /// Gets the name of this mod.
        /// </summary>
        public string Name => "Snooper " + modVersion;

        /// <summary>
        /// Gets the description string of this mod.
        /// </summary>
        public string Description => "Shows additional information about citizens and tourists.";

        /// <summary>Called when this mod is enabled.</summary>
        public void OnEnabled() => Log.Msg("Mod has been enabled, version: " + modVersion);

        /// <summary>Called when this mod is disabled.</summary>
        public void OnDisabled() => Log.Msg("Mod has been disabled.");

        /// <summary>
        /// Called when a game level is loaded. If applicable, activates the Snooper mod
        /// for the loaded level.
        /// </summary>
        ///
        /// <param name="mode">The <see cref="LoadMode"/> a game level is loaded in.</param>
        public override void OnLevelLoaded(LoadMode mode)
        {
            switch (mode)
            {
                case LoadMode.LoadGame:
                case LoadMode.NewGame:
                case LoadMode.LoadScenario:
                case LoadMode.NewGameFromScenario:
                    break;

                default:
                    return;
            }
            try
            {
                HarmonyPatcher.PatchOnReady(ModAssembly, HarmonyId);
            }
            catch (System.IO.FileNotFoundException e)
            {
                Log.Err("Assembly of Harmony is missing: " + e.Message);
            }
            WorldInfoPanelPatches.CitizenInfoPanel = CustomCitizenInfoPanel.Enable();
            WorldInfoPanelPatches.TouristInfoPanel = CustomTouristInfoPanel.Enable();
            WorldInfoPanelPatches.CitizenVehicleInfoPanel = CustomCitizenVehicleInfoPanel.Enable();
            WorldInfoPanelPatches.ServiceVehicleInfoPanel = CustomCityServiceVehicleInfoPanel.Enable();
        }

        /// <summary>
        /// Called when a game level is about to be unloaded. If the Snooper mod was activated
        /// for this level, deactivates the mod for this level.
        /// </summary>
        public override void OnLevelUnloading()
        {
            HarmonyPatcher.TryUnpatch(HarmonyId);

            WorldInfoPanelPatches.CitizenInfoPanel?.Disable();
            WorldInfoPanelPatches.CitizenInfoPanel = null;

            WorldInfoPanelPatches.TouristInfoPanel?.Disable();
            WorldInfoPanelPatches.TouristInfoPanel = null;

            WorldInfoPanelPatches.CitizenVehicleInfoPanel?.Disable();
            WorldInfoPanelPatches.CitizenVehicleInfoPanel = null;

            WorldInfoPanelPatches.ServiceVehicleInfoPanel?.Disable();
            WorldInfoPanelPatches.ServiceVehicleInfoPanel = null;
        }
        private Assembly ModAssembly => Assembly.GetExecutingAssembly();
    }
}
