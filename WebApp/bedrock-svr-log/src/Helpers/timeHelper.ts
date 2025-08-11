export const formatTimeCount = (time: number) => {
    const workingTime = time.toString().split(":");
    return workingTime[0] + "h " + workingTime[1] + "m " + workingTime[2].slice(0, 2) + "s";
};

export const formatTime = (input: string) => {
    const [hourStr, minuteStr, secondStr] = input.split(':');
    let hours = parseInt(hourStr, 10);
    const minutes = minuteStr.padStart(2, '0');
    const seconds = secondStr.split('.')[0].padStart(2, '0'); // ignore milliseconds

    // Determine AM or PM
    const ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12 || 12; // Convert 0 → 12

    return `${hours}:${minutes}:${seconds} ${ampm}`;
};

export const formatDateTime = (input: string) => {    
    const date = new Date(input);

    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Month is 0-indexed
    const year = date.getFullYear();

    const time = formatTime(input.split("T")[1])
    return `${day}/${month}/${year} ${time}`; 
}

export const getTimeDifferenceTimeDateFull = (currentDateTime: Date, time: string) => {
    // const now = new Date();
    const spawnDate = new Date(time);
    const timeDiff = currentDateTime.getTime() - spawnDate.getTime();
    //convert to hours, minutes, seconds
    const hours = Math.floor(timeDiff / (1000 * 60 * 60));
    const minutes = Math.floor((timeDiff % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((timeDiff % (1000 * 60)) / 1000);
    return `${hours}h ${minutes}m ${seconds}s`;
  }
