using HarmonyLib;
using UnityEngine;


namespace Pseudonym;

// Patches class
public class Pseudonym
{
    // set at runtime, probably dumb and bad but i dont really care!
    public static string Nickname = "^_^";
    public static bool ObscureSteamId = false;

    [HarmonyPatch(typeof(NetworkConnector), "GetUsername")] // Specify target method with HarmonyPatch attribute
    [HarmonyPrefix]
    static bool GetPseudonym(ref string __result)
    {
        __result = Nickname; // The special __result variable allows you to read or change the return value
        return false; // Returning false in prefix patches skips running the original code
    }

    [HarmonyPatch(typeof(NetworkConnector), "HandleConnectionState")]
    [HarmonyPrefix]
    static bool LeaveLobbyAfterJoin(object[] __args)
    {
        // only do this if obscuresteamid is true
        if (ObscureSteamId)
        {
            if (__args.Length > 0)
            {
                if (__args[0] is JoinSpecificRoomState state)
                {
                    GameHandler.GetService<SteamLobbyHandler>().LeaveLobby();
                    Debug.Log("[Pseudonyms] Left steam lobby.");
                }
                else
                {
                    Debug.Log("[Pseudonyms] State is not JoinSpecificRoomState.");
                }
            }
            else
            {
                Debug.Log("[Pseudonyms] For some reason state is null.");
            }
        }
        return true;
    }
}
