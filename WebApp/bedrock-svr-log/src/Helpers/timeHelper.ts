type MinecraftWorldInfo = {
  ticks: number;
  totalTicks: number;
  time: string;
  isDay: boolean;
  isDayIcon: string;
  timeColor: SkyColor;
};

type SkyColor = {
    color1: string;
    color2: string;
  };

type AmPmTime = {
  hours: string;
  minutes: string;
  amPm: string;
  noTimeYet: boolean;
};

export const formatTimeCount = (time: string) => {
  const workingTime = time.toString().split(":");
  if (workingTime[0].includes(".")) {
    const days = workingTime[0].split(".")[0];
    const hours = workingTime[0].split(".")[1];
    const minutes = workingTime[1];
    const seconds = workingTime[2].slice(0, 2);
    return days + "d " + hours + "h " + minutes + "m " + seconds + "s";
  }

  return (
    workingTime[0] +
    "h " +
    workingTime[1] +
    "m " +
    workingTime[2].slice(0, 2) +
    "s"
  );
};

export const formatTime = (input: string) => {
  const [hourStr, minuteStr, secondStr] = input.split(":");
  let hours = parseInt(hourStr, 10);
  const minutes = minuteStr.padStart(2, "0");
  const seconds = secondStr.split(".")[0].padStart(2, "0"); // ignore milliseconds

  // Determine AM or PM
  const ampm = hours >= 12 ? "PM" : "AM";
  hours = hours % 12 || 12; // Convert 0 → 12

  return `${hours}:${minutes}:${seconds} ${ampm}`;
};

export const formatDateTime = (input: string) => {
  const date = new Date(input);

  const day = String(date.getDate()).padStart(2, "0");
  const month = String(date.getMonth() + 1).padStart(2, "0"); // Month is 0-indexed
  const year = date.getFullYear();

  const time = formatTime(input.split("T")[1]);
  return `${day}/${month}/${year} ${time}`;
};

export const getTimeDifferenceTimeDateFull = (
  currentDateTime: Date,
  time: string
): string => {
  // const now = new Date();
  const spawnDate = new Date(time);
  const timeDiff = currentDateTime.getTime() - spawnDate.getTime();
  //convert to hours, minutes, seconds
  const hours = Math.floor(timeDiff / (1000 * 60 * 60));
  const minutes = Math.floor((timeDiff % (1000 * 60 * 60)) / (1000 * 60));
  const seconds = Math.floor((timeDiff % (1000 * 60)) / 1000);
  
  return `${hours}h ${minutes}m ${seconds}s`;
};

export const minecraftTimeDayConvert = (ticks: number): MinecraftWorldInfo => {
  // Normalize to 0 - 23999
  const normalizedTicks = ticks % 24000;

  const isDay = normalizedTicks >= 0 && normalizedTicks < 12000;
  const isDayIcon = isDay ? "☀️" : "🌙";

  const { hours, minutes, amPm } = minecraftTicksToTimeObject(normalizedTicks);
  const time = `${hours}:${minutes} ${amPm}`;

  return {
    ticks: normalizedTicks,
    totalTicks: ticks,
    time: time,
    isDay: isDay,
    isDayIcon: isDayIcon,
    timeColor: dayNightSkyColor(normalizedTicks),
  } as MinecraftWorldInfo;
};

export const minecraftTicksToTimeObject = (ticks: number): AmPmTime => {
  // Normalize to 0 - 23999
  const normalizedTicks = ticks % 24000;

  // Minecraft day starts at 0 ticks = 6:00 AM, so offset by +6000 ticks
  let hours = Math.floor((normalizedTicks + 6000) / 1000) % 24;
  const minutes = Math.floor((((normalizedTicks + 6000) % 1000) / 1000) * 60);

  if (isNaN(hours) || isNaN(minutes)) {
    return {
      hours: "00",
      minutes: "00",
      amPm: "AM",
      noTimeYet: true,
    } as AmPmTime;
  }

  // Convert 24-hour format to 12-hour AM/PM format
  const amPm = hours >= 12 ? "PM" : "AM";
  hours = hours % 12;
  if (hours === 0) hours = 12;

  const formattedMinutes = minutes.toString().padStart(2, "0");
  const formattedHours = hours.toString();
   
  return {
    hours: formattedHours,
    minutes: formattedMinutes,
    amPm: amPm,
    noTimeYet: false,
  } as AmPmTime;
};

export const minecraftTicksToTime = (ticks: number): string => {
  const { hours, minutes, amPm } = minecraftTicksToTimeObject(ticks);
  return `${hours}:${minutes} ${amPm}`;
};

/*
Minecraft real time to ticks:

6:00 AM → 0 ticks               "3:00 PM" → 9000,               "12:00 AM" → 18000,
7:00 AM → 1,000 ticks           "4:00 PM" → 10000,               "1:00 AM" → 19000,
8:00 AM → 2,000 ticks           "5:00 PM" → 11000,               "2:00 AM" → 20000,
9:00 AM → 3,000 ticks           "6:00 PM" → 12000,               "3:00 AM" → 21000,
10:00 AM → 4,000 ticks          "7:00 PM" → 13000,               "4:00 AM" → 22000,
11:00 AM → 5,000 ticks          "8:00 PM" → 14000,               "5:00 AM" → 23000,
12:00 PM → 6,000 ticks          "9:00 PM" → 15000,               "6:00 AM" → 24,000 ticks (wraps back to 0)
1:00 PM → 7,000 ticks           "10:00 PM" → 16000,              
2:00 PM → 8,000 ticks           "11:00 PM" → 17000,
*/


const dayNightSkyColor = (ticks: number): SkyColor => {
  // Normalize ticks to always stay within 0 → 23999
  const normalizedTicks = ticks % 24000;

  // Night: 7 PM → 4 AM (13000 → 22000)
  if (normalizedTicks >= 13000 && normalizedTicks < 22000) {
    return { color1: "#1919D0", color2: "#000000" }; // Dark Blue

    // Morning: 4 AM → 6 AM (22000 → 24000) AND (0 → 2000)
  } else if (normalizedTicks >= 22000 || normalizedTicks < 2000) {
    return { color1: "#7060C0", color2: "#55AAD5" }; // Pinkish Blue

    // Day: 6 AM → 5 PM (2000 → 11000)
  } else if (normalizedTicks >= 2000 && normalizedTicks < 11000) {
    return { color1: "#88BBEE", color2: "#FFFFFF" }; // Light Blue

    // Evening: 5 PM → 7 PM (11000 → 13000)
  } else if (normalizedTicks >= 11000 && normalizedTicks < 13000) {
    return { color1: "#9370DB", color2: "#3311AA" }; // Purplish Blue

    // Fallback (should never happen)
  } else {
    return { color1: "#000000", color2: "#000000" };
  }
};
