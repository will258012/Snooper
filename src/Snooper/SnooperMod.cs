// <copyright file="SnooperMod.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace Snooper
{
    using ICities;
    using Snooper.Panels;
    using Snooper.Patches;
    using WillCommons;

    /// <summary>
    /// The main class of the Snooper mod.
    /// </summary>
    public sealed class SnooperMod : PatcherModBase
    {
        public override string BaseName => "Snooper";
        public override string Description => "Shows additional information about citizens and tourists.";
        public override string HarmonyID => "com.cities_skylines.dymanoid.snooper";

        /// <summary>Called when this mod is enabled.</summary>
        public override void OnEnabled()
        {
            base.OnEnabled();
            Logging.Msg("Mod has been enabled, version: " + ModVersion);
        }

        /// <summary>Called when this mod is disabled.</summary>
        public override void OnDisabled()
        {
            base.OnDisabled();
            Logging.Msg("Mod has been disabled.");
        }

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
            WorldInfoPanelPatches.CitizenInfoPanel?.Disable();
            WorldInfoPanelPatches.CitizenInfoPanel = null;

            WorldInfoPanelPatches.TouristInfoPanel?.Disable();
            WorldInfoPanelPatches.TouristInfoPanel = null;

            WorldInfoPanelPatches.CitizenVehicleInfoPanel?.Disable();
            WorldInfoPanelPatches.CitizenVehicleInfoPanel = null;

            WorldInfoPanelPatches.ServiceVehicleInfoPanel?.Disable();
            WorldInfoPanelPatches.ServiceVehicleInfoPanel = null;
        }
    }
}
