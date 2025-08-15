using Il2CppInterop.Runtime.Injection;
using MelonLoader;

namespace MapSpeed;

internal class Main : MelonMod
{
    public static MapSpeed MapSpeed { get; private set; }

    public override void OnLateInitializeMelon()
    {
        MapSpeed = new MapSpeed();
        ClassInjector.RegisterTypeInIl2Cpp<MapSpeedLogic>();
    }
}