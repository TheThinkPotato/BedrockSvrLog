﻿import { system, world } from "@minecraft/server";

// Run every 30 seconds (600 ticks, since 20 ticks = 1 second)
const INTERVAL = 30 * 20;

system.runInterval(() => {
  try {
    // Get all players in the world
    const players = world.getPlayers();
    const timeOfDay = world.getTimeOfDay();
    const dayNumber = world.getDay();

    console.log(`[TIME] Day: ${dayNumber} Time of Day: ${timeOfDay}`);

    if (players.length === 0) {
      return;
    }

    for (const player of players) {
      try {
        // Get player's name
        const name = player.name;

        // Get player's current location
        const { x, y, z } = player.location;

        // Get player's dimension (Overworld, Nether, End)
        const dimension = player.dimension.id;

        console.log(
          `[TRACKING] ${name} is at X:${Math.floor(x)}, Y:${Math.floor(
            y
          )}, Z:${Math.floor(z)} in ${dimension}`
        );
      } catch (playerError) {
        console.error(`Error tracking player: ${player.name}`, playerError);
      }
    }
  } catch (err) {
    console.error("Failed to track players:", err);
  }
}, INTERVAL);

world.afterEvents.entityDie.subscribe((eventData) => {
  const deadEntity = eventData.deadEntity;

  const getLocation = () => {
    if (deadEntity) {
      const location = {
        x: Math.floor(deadEntity.location.x),
        y: Math.floor(deadEntity.location.y),
        z: Math.floor(deadEntity.location.z),
      };

      const dimension = deadEntity.dimension;

      return `${location.x}, ${location.y}, ${location.z} in ${dimension.id}`;
    }
    return "Unknown Location";
  };

  const getKiller = () => {
    const killer = eventData.damageSource?.damagingEntity;
    const cause = eventData.damageSource?.cause;
    const causeProjectile = eventData.damageSource?.projectile;

    if (killer) {
      if (killer.typeId === "minecraft:player") {
        return `player - ${killer.nameTag}`;
      } else if (killer.typeId) {
        return killer.typeId;
      }
    }
    if (cause) {
      return cause;
    }
    if (causeProjectile) {
      return causeProjectile.typeId;
    }
    return "Unknown";
  };

  // If it's a player that died
  if (deadEntity.typeId === "minecraft:player") {
    console.log(
      `Script Player Death: ${
        deadEntity.nameTag
      } at: ${getLocation()} by: ${getKiller()}`
    );
  } else {
    // If it’s an animal (or any mob)
    console.log(
      `Script Entity Death: ${
        deadEntity.typeId
      } at: ${getLocation()} by: ${getKiller()}`
    );
  }
});
