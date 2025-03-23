// -----------------------------------------------------------------------
// <copyright file="Scp2818.cs" company="Joker119">
// Copyright (c) Joker119. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace CustomItems.Items;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using UnityEngine;
using YamlDotNet.Serialization;

/// <summary>
/// A gun that kills you.
/// </summary>
[CustomItem(ItemType.GunE11SR)]
public class Scp2818 : CustomWeapon
{
    /// <inheritdoc/>
    public override uint Id { get; set; } = 14;

    /// <inheritdoc/>
    public override string Name { get; set; } = "SCP-2818";

    /// <inheritdoc/>
    public override string Description { get; set; } =
        "When this weapon is fired, it uses the biomass of the shooter as the bullet.";

    /// <inheritdoc/>
    public override float Weight { get; set; } = 3.95f;

    /// <inheritdoc/>
    [YamlIgnore]
    public override byte ClipSize { get; set; } = 1;

    /// <summary>
    /// Gets or sets how often the <see cref="ShooterProjectile"/> coroutine will move the player.
    /// </summary>
    [Description(
        "How frequently the shooter will be moved towards his target.\n# Note, a lower tick frequency, and lower MaxDistance will make the travel smoother, but be more stressful on your server.")]
    public float TickFrequency { get; set; } = 0.00025f;

    /// <summary>
    /// Gets or sets the max distance towards the target location the shooter can be moved each tick.
    /// </summary>
    [Description("The max distance towards the target location the shooter can be moved each tick.")]
    public float MaxDistancePerTick { get; set; } = 0.50f;

    /// <summary>
    /// Gets or sets a value indicating whether the gun should despawn instead of drop when it is fired.
    /// </summary>
    [Description("Whether or not the weapon should despawn itself after it's been used.")]
    public bool DespawnAfterUse { get; set; } = false;

    /// <inheritdoc/>
    public override SpawnProperties? SpawnProperties { get; set; } = new()
    {
        Limit = 1,
        DynamicSpawnPoints = new List<DynamicSpawnPoint>
        {
            new()
            {
                Chance = 60,
                Location = SpawnLocationType.InsideHidChamber,
            },
            new()
            {
                Chance = 40,
                Location = SpawnLocationType.InsideHczArmory,
            },
        },
    };

    /// <inheritdoc/>
    [Description("The amount of damage the weapon deals when the projectile hits another player.")]
    public override float Damage { get; set; } = float.MaxValue;

    public string DeathReasonUser { get; set; } = "Vaporized by becoming a bullet";
    public string DeathReasonTarget { get; set; } = "Vaporized by a human bullet";

    /// <inheritdoc/>
    protected override void OnShot(ShotEventArgs ev)
    {
        if (ev.Target == null)
        {
            Log.Debug(
                $"VVUP Custom Items: SCP-2818, {ev.Player.Nickname} fired and missed a target, teleporting them to bullet impact location ({ev.Position}");
            ev.Player.Position = ev.Position;
        }
        else
        {
            Log.Debug(
                $"VVUP Custom Items: SCP-2818, {ev.Player.Nickname} shot and hit {ev.Target.Nickname}, running hit code");
            ev.CanHurt = false;
            ev.Player.Position = ev.Target.Position;
            if (ev.Target.Health <= Damage)
            {
                Log.Debug(
                    $"VVUP Custom Items: SCP-2818, {ev.Target.Nickname} has {ev.Target.Health} but damage is set to {Damage}. Killing {ev.Target.Nickname}");
                ev.Target.Kill(DeathReasonTarget);
            }
            else
            {
                Log.Debug(
                    $"VVUP Custom Items: SCP-2818, {ev.Target.Nickname} has {ev.Target.Health} which is higher than {Damage}, dealing {Damage} to {ev.Target.Nickname}");
                ev.Target.Hurt(Damage);
            }
        }

        if (DespawnAfterUse)
        {
            Log.Debug(
                $"VVUP Custom Items: SCP-2818, Despawn After Use is true, removing SCP-2818 from {ev.Player.Nickname}'s inventory");
            ev.Player.RemoveItem(ev.Item);
        }

        Timing.CallDelayed(0.1f, () =>
        {
            Log.Debug($"VVUP Custom Items: SCP-2818, Killing {ev.Player.Nickname}");
            ev.Player.Kill(DeathReasonUser);
        });
    }
}