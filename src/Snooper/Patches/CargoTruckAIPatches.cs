// <copyright file="CargoTruckAIPatches.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace Snooper.Patches
{
    using HarmonyLib;

    /// <summary>
    /// A static class that provides the patch objects for the car AI game methods.
    /// </summary>
    [HarmonyPatch]
    internal static class CargoTruckAIPatches
    {
        /// <summary>Gets the patch for the set target method.</summary>
        [HarmonyPatch(typeof(CargoTruckAI), nameof(CargoTruckAI.SetTarget))]
        private static void Prefix(ref Vehicle data, ref ushort __state) => __state = data.m_targetBuilding;

        [HarmonyPatch(typeof(CargoTruckAI), nameof(CargoTruckAI.SetTarget))]
        private static void Postfix(ref Vehicle data, ushort targetBuilding, ref ushort __state)
        {
            if (__state != 0 && targetBuilding == 0 && data.m_touristCount == 0 && (data.m_flags & Vehicle.Flags.GoingBack) != 0)
            {
                // Storing the original target building ID in the tourist count field.
                // It won't be used by the cargo truck AI, so no side effects.
                // Storing it back into the target building field might lead to side effects though.
                data.m_touristCount = __state;
            }
        }
    }
}
