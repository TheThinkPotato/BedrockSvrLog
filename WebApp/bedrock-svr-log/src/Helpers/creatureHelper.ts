// Helper function to get player's kill status
export function getKillStatus(kills: number, creature: string): string {
  // Make sure kills stay within range 1–400
  const safeKills = Math.max(1, Math.min(kills, 400));

  if (safeKills < 20) {
    return "Human";
  }

  // Status names for different kill milestones
  const statuses = ["Farmer", "Hunter", "Assassin", "Reaper", "Slayer"];

  // Each tier covers 40 kills
  const index = Math.min(Math.floor((safeKills - 1) / 80), statuses.length - 1);
  
  return `${creatureNameCleanUp(creature)} ${statuses[index]}`;
}

export function creatureNameCleanUp(creature: string): string {
  return (creature.charAt(0).toUpperCase() + creature.slice(1))
    .replace("_", " ")
    .replace("_", " ");
}
