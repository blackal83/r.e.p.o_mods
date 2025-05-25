using HarmonyLib;
using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace LegioFenixRevive;

[HarmonyPatch(typeof(PhysGrabObject), "FixedUpdate")]
public static class LegioFenixRevivePatch
{
    private static void Prefix(ref PhysGrabObject __instance)
    {
        if (Input.GetKeyDown((KeyCode)114))
        {
            
            //Debug.Log("[Reviver] Holding Key Down.....");
            if (__instance.name.StartsWith("Player Death Head") && __instance.grabbedLocal)
            {
                PlayerDeathHead component = __instance.GetComponent<PlayerDeathHead>();
                if (component != null)
                {
                    PlayerAvatar deadPlayerAvatar = component.playerAvatar;
                    PlayerHealth deadPlayerHealth = deadPlayerAvatar.playerHealth;
                    string deadPlayerName = (string)AccessTools.Field(typeof(PlayerAvatar), "playerName").GetValue(deadPlayerAvatar);

                    PlayerAvatar reviverAvatar = PlayerAvatar.instance;
                    PlayerHealth reviverHealth = reviverAvatar.playerHealth;

                    int reviverHealthValue = (int)AccessTools.Field(typeof(PlayerHealth), "health").GetValue(reviverHealth);
                    int healthToTransfer = (int)Math.Floor(reviverHealthValue * LegioFenixReviveBase.percentOfHealthToTransfer.Value);

                    if (deadPlayerAvatar != null)
                    {
                        if (reviverHealthValue >= LegioFenixReviveBase.minHealthRequiredToRevive.Value) 
                        {
                            Debug.Log("[Reviver] Attempting to revive: " + deadPlayerName);
                            deadPlayerAvatar.Revive(false);
                            deadPlayerHealth.HealOther(healthToTransfer, true);
                            reviverHealth.Hurt(healthToTransfer, true, -1);
                            Debug.Log("[Reviver] Revived: " + deadPlayerName);
                            reviverAvatar.ChatMessageSpeak("I HAVE REVIVED THE RETARD " + deadPlayerName + "!! BEHOLD MY AWESOMENESS!!!", false);
                        }
                        else
                        {
                            reviverAvatar.ChatMessageSpeak("I TRIED TO REVIVE " + deadPlayerName + " BUT I AM TOO WEAK!!!", false);
                        }
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