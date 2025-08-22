import { system, world, EntityTypes } from "@minecraft/server";
  const dgram = require("dgram");
  const client = dgram.createSocket("udp4");


const bedrockLoggerSend = (message) => {
  const msgBuffer = Buffer.from(message);
  client.send(msgBuffer, 5550, "127.0.0.1", (err) => {
    if (err) throw err;
  });
};

world.afterEvents.entityDie.subscribe((eventData) => {
  const entity = eventData.deadEntity;
  const killer = eventData.damageSource?.damagingEntity;

  // If it's a player that died
  if (entity.typeId === "minecraft:player") {
    bedrockLoggerSend(
      `Script Player: ${entity.nameTag} has died! at ${eventData.location} by ${eventData.damageSource?.typeId}`
    );
  } else {
    // If it’s an animal (or any mob)
    bedrockLoggerSend(`Script Entity Died: ${entity.typeId}`);
  }

  // Bonus: if there was a killer
  if (killer) {
    if (killer.typeId === "minecraft:player") {
      bedrockLoggerSend(`Script Killed by player: ${killer.nameTag}`);
    } else {
      bedrockLoggerSend(`Script Killed by: ${killer.typeId}`);
    }
  }
});
