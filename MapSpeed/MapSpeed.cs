using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppTGK.Game.Components.UI;
using UnityEngine;

namespace MapSpeed;

public class MapSpeed : BlasIIMod
{
    internal MapSpeed() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    public static float Speed { get; internal set; } = 1;

    protected override void OnInitialize()
    {
        LocalizationHandler.RegisterDefaultLanguage("en");
    }
}

[HarmonyPatch(typeof(MapWindowLogic), "OnShow")]
public class MapWindowLogic_OnShow_Patch
{
    public static void Postfix(MapWindowLogic __instance)
    {
        if (MapSpeedLogic.Instance == null) __instance.gameObject.AddComponent<MapSpeedLogic>().Setup();
    }
}

[HarmonyPatch(typeof(MapWindowLogic), "MoveMap")]
public class MapWindowLogic_MoveMap_Patch
{
    public static void Prefix(ref Vector2 desiredMovement)
    {
        desiredMovement *= MapSpeed.Speed;
    }
}