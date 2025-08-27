import React, { useEffect, useRef, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { Box } from "@mui/material";
import { minecraftTicksToTime, minecraftTimeDayConvert } from "../Helpers/timeHelper";
import { dataHubConnection } from "../Api/Api";

export type WorldData = {
  Id: number;
  Name: string;
  CurrentTime: number;
  CurrentDay: number;
  Seed: string;
};

const SignalRTest: React.FC = () => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null
  );
  const [messages, setMessages] = useState<string[]>([]);

  const [shownTime,setShownTime] = useState<number>(0);

  useEffect(() => {
    setConnection(dataHubConnection);
  }, []);

  useEffect(() => {
    if (!connection) return;

    connection
      .start()
      .then(() => {
        console.log("Connected to SignalR Hub");

        // Listen for Data from server
        connection.on("ReceiveWorldData", (message) => {
          setMessages((prev) => [...prev, message]);
        });

        // Send ping every 30 seconds
        setInterval(() => {
          connection.invoke("SendWorldData");
        }, 30 * 1000);
      })
      .catch((err) => console.error("Connection failed: ", err));
  }, [connection]);

  const lastMessage: WorldData | undefined =
    messages.length > 0
      ? (JSON.parse(messages[messages.length - 1]) as WorldData)
      : undefined;

  // if lastMessage is undefined, get world data from server
  const currentDay = lastMessage?.CurrentDay ?? connection?.invoke("SendWorldData");

  const {
    ticks: currentTicks,
    isDayIcon,
    timeColor,
  } = minecraftTimeDayConvert(lastMessage?.CurrentTime ?? NaN);

  // prevTicks is used to sync the shownTime with the currentTicks
  const prevTicks = useRef<number>(currentTicks);

  // update shownTime every 250ms
  useEffect(() => {
    const interval = setInterval(() => {
      if (isNaN(shownTime) || shownTime === 0) {
        setShownTime(currentTicks);
      }    

      // if current ticks != prev ticks then
      if (currentTicks !== prevTicks.current) {
        prevTicks.current = currentTicks;
        setShownTime(currentTicks);
      }

      if (shownTime !== 0) {
        setShownTime((prev) => prev + 5);
      }
    }, 255);
    return () => clearInterval(interval);
  }, [currentTicks, shownTime]);

  return (
    <Box
      className="fixed z-50"
      style={{
        left: "3rem",
        top: "1.35rem",
        backgroundColor: "rgba(255, 255, 255, 1)",
        minWidth: "160px",
        height: "33px",
        borderRadius: "0.2rem",
        boxShadow: "3px 3px 2px rgba(0, 0, 0, 0.2)",
        color: "black",
        fontFamily:
          "-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif",
      }}
    >
      <Box className="flex flex-col gap-2 overflow-y-auto max-h-[200px]">
        <Box
          className="text-sm"
          style={{
            fontSize: "16px",
            paddingBlock: "0.2rem",
            paddingInline: "0.5rem",
          }}
        >
          <Box className="flex flex-row" style={{ gap: "0.5rem" }}>
            <Box
              style={{
                background: `linear-gradient(to top, ${timeColor.color1}, ${timeColor.color2})`,
                marginTop: "0.1rem",
                textAlign: "center",
              }}
            >
              {`${isDayIcon}`}
            </Box>
            {`Day ${currentDay} | ${minecraftTicksToTime(shownTime)}`}
          </Box>
        </Box>
      </Box>
    </Box>
  );
};

export default SignalRTest;
