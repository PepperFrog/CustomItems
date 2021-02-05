using System.Collections.Generic;
using System.ComponentModel;
using CustomItems.API;

namespace CustomItems.ItemConfigs
{
    public class Scp127Config
    {
        [Description("How often ammo will be regenerated. Regeneration occurs at all times, however this timer is reset when the weapon is picked up or dropped.")]
        public float RegenDelay { get; set; } = 10f;
        
        [Description("The amount of ammo that will be regenerated each regeneration cycle.")]
        public int RegenAmount { get; set; } = 2;
        
        [Description("The max clip size for the weapon.")]
        public int ClipSize { get; set; } = 12;
        
        [Description("The base weapon this one will be modeled after.")]
        public ItemType ItemType { get; set; } = ItemType.GunCOM15;

        [Description("Where on the map items should spawn, and their % chance of spawning in each location.")]
        public Dictionary<SpawnLocation, float> SpawnLocations { get; set; } = new Dictionary<SpawnLocation, float>
        {
            { SpawnLocation.Inside173Armory, 100 }
        };

        [Description("The Custom Item ID for this item.")]
        public int Id { get; set; } = 7;
        
        [Description("The description of this item show to players when they obtain it.")]
        public string Description { get; set; } = "SCP-127 is a pistol that slowly regenerates it's ammo over time but cannot be reloaded normally.";

        [Description("The name of this item shown to players when they obtain it.")]
        public string Name { get; set; } = "SCP-127";
        
        [Description("How many of this item are allowed to naturally spawn on the map when a round starts. 0 = unlimited")]
        public int SpawnLimit { get; set; } = 1;
    }
}