// We have made a new file for this configuration class
using System.Reflection;
using System.Collections.Generic;
using HarmonyLib;
using BepInEx.Configuration;

namespace Pseudonym;

class PseudonymConfig
{
    // We define our config variables in a public scope
    public readonly ConfigEntry<string> Nickname;
    public readonly ConfigEntry<bool> ObscureSteamId;

    public PseudonymConfig(ConfigFile cfg)
    {
        cfg.SaveOnConfigSet = false;

        Nickname = cfg.Bind(
            "General",                  // Config subsection
            "Nickname",                  // Key of this config
            "^_^",                               // Default value
            "The nickname to use."         // Description
        );

        ObscureSteamId = cfg.Bind(
            "General",                  // Config subsection
            "ObscureSteamId",                  // Key of this config
            false,                               // Default value
            "Leaves the steam lobby right after receiving a photon lobby ID, potentially obscuring your steam ID. Players may still get your steam ID, but it will be more difficult to link it to your nickname if they don't match."         // Description
        );

        // Get rid of old settings from the config file that are not used anymore //
        ClearOrphanedEntries(cfg);
        // We need to manually save since we disabled `SaveOnConfigSet` earlier //
        cfg.Save();
        // And finally, we re-enable `SaveOnConfigSet` so changes to our config //
        // entries are written to the config file automatically from now on //
        cfg.SaveOnConfigSet = true;
    }
    
    static void ClearOrphanedEntries(ConfigFile cfg) 
    { 
        // Find the private property `OrphanedEntries` from the type `ConfigFile` //
        PropertyInfo orphanedEntriesProp = AccessTools.Property(typeof(ConfigFile), "OrphanedEntries"); 
        // And get the value of that property from our ConfigFile instance //
        var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(cfg); 
        // And finally, clear the `OrphanedEntries` dictionary //
        orphanedEntries.Clear(); 
    } 
}