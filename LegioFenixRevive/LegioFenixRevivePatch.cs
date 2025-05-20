using HarmonyLib;
using UnityEngine;

namespace LegioFenixRevive;

[HarmonyPatch(typeof(PhysGrabObject), "FixedUpdate")]
public static class LegioFenixRevivePatch
{
    private static void Prefix(ref PhysGrabObject __instance)
    {
        if (Input.GetKeyDown((KeyCode)114))
        {
            Debug.Log("[Reviver] Holding Key Down.....");
            if (__instance.name.StartsWith("Player Death Head") && __instance.grabbedLocal)
            {
                PlayerDeathHead component = __instance.GetComponent<PlayerDeathHead>();
                if (component != null)
                {
                    PlayerAvatar deadPlayerAvatar = component.playerAvatar;
                    PlayerHealth deadPlayerHealth = deadPlayerAvatar.playerHealth;
                    string deadPlayerName = (string)AccessTools.Field(typeof(PlayerAvatar), "playerName").GetValue(deadPlayerAvatar);

                    PlayerAvatar instance = PlayerAvatar.instance;
                    PlayerHealth reviverHealth = instance.playerHealth;

                    int reviverHealthValue = (int)AccessTools.Field(typeof(PlayerHealth), "health").GetValue(reviverHealth);
                    int healthToTransfer = LegioFenixReviveBase.healthToTransfer.Value;

                    if (deadPlayerAvatar != null && reviverHealthValue > healthToTransfer)
                    {
                        Debug.Log("[Reviver] Attempting to revive: " + deadPlayerName);
                        deadPlayerAvatar.Revive(false);
                        deadPlayerHealth.HealOther(healthToTransfer, true);
                        reviverHealth.Hurt(healthToTransfer, true, -1);
                        Debug.Log("[Reviver] Revived: " + deadPlayerName);
                    }
                    else
                    {
                        Debug.Log("[Reviver] Not able to revive: " + deadPlayerName);
                    }
                }
            }
        }
    }
}