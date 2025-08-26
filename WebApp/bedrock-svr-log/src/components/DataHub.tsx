import React, { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { Box } from "@mui/material";

export type WorldData = {
  Id: number;
  Name: string;
  CurrentTime: string;
  CurrentDay: number;
  Seed: string;
};

const SignalRTest: React.FC = () => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null
  );
  const [messages, setMessages] = useState<string[]>([]);

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5000/DataHub") // C# SignalR URL
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (!connection) return;

    connection
      .start()
      .then(() => {
        console.log("Connected to SignalR Hub");

        // Listen for Pong response
        connection.on("ReceiveWorldData", (message) => {
          // console.log("Received from server:", message);
          setMessages((prev) => [...prev, message]);
        });

        // Send ping every 30 seconds
        setInterval(() => {
          connection.invoke("SendWorldData");
        }, 5000);
      })
      .catch((err) => console.error("Connection failed: ", err));
  }, [connection]);

  const lastMessage: WorldData | undefined =
    messages.length > 0
      ? (JSON.parse(messages[messages.length - 1]) as WorldData)
      : undefined;

  return (
    <Box
      className="fixed z-50"
      style={{
        top: "0.56rem",
        right: "11rem",
        backgroundColor: "rgba(255, 255, 255, 1)",
        width: "300px",
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
          {`Day ${lastMessage?.CurrentDay} - ${lastMessage?.CurrentTime}`}
        </Box>
      </Box>
    </Box>
  );
};

export default SignalRTest;
