export const formatTime = (time: number) => {
    const workingTime = time.toString().split(":");
    return workingTime[0] + "h " + workingTime[1] + "m " + workingTime[2].slice(0, 2) + "s";
};

export const formatDateTime = (dateTime: string, show24Hour = false) => {
    let workingDate = dateTime.split("T")[0];
    workingDate = workingDate.split("-").reverse().join("/");
    const workingTime = dateTime.split("T")[1].split(":");
    if (show24Hour) {
        return workingDate + " " + workingTime[0] + ":" + workingTime[1] + ":" + workingTime[2].slice(0, 2);
    } else {
        return workingDate + " " + workingTime[0] + ":" + workingTime[1] + ":" + workingTime[2].slice(0, 2) + " " + (parseInt(workingTime[0]) < 12 ? "AM" : "PM");
    }
};

export const formatDateTimeFull = (dateTime: string, show24Hour = false) => {
    let workingDate = dateTime.split("T")[0];
    workingDate = workingDate.split("-").reverse().join("/");
    const workingTime = dateTime.split("T")[1].split(":");
    if (show24Hour) {
        return workingDate + " " + workingTime[0] + ":" + workingTime[1] + ":" + workingTime[2].slice(0, 2);
    } else {
        return workingDate + " " + workingTime[0] + ":" + workingTime[1] + ":" + workingTime[2].slice(0, 2) + " " + (parseInt(workingTime[0]) < 12 ? "AM" : "PM");
    }
}
