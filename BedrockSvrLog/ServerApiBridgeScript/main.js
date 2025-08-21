import { system, world, EntityTypes } from "@minecraft/server";

world.afterEvents.entityDie.subscribe((eventData) => {
const entity = eventData.deadEntity;
const killer = eventData.damageSource?.damagingEntity;

// If it's a player that died
if (entity.typeId === "minecraft:player") {
    console.log(`Scipt Player: ${entity.nameTag} has died! at ${eventData.location} by ${eventData.damageSource?.typeId}`);
    } else {
        // If it’s an animal (or any mob)
        console.log(`Scipt Enity Died: ${entity.typeId}`);
    }

// Bonus: if there was a killer
if (killer) {
        if (killer.typeId === "minecraft:player") {
        console.log(`Script Killed by player: ${killer.nameTag}`);
        } else {
        console.log(`Script Killed by: ${killer.typeId}`);
        }
    }
});
