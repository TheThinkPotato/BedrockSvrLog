import { world } from "@minecraft/server";

world.afterEvents.entityDie.subscribe((eventData) => {
  const deadEntity = eventData.deadEntity;

  const getLocation = () => {
    if (deadEntity) {
      const location = {
        x: deadEntity.location.x,
        y: deadEntity.location.y,
        z: deadEntity.location.z,
      };
      return location;
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
