// src/context/WorldContext.tsx
import React, { createContext, useContext, useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { dataHubConnection } from "../Api/Api";

export type WorldData = {
  Name: string;
  CurrentTime: number;
  CurrentDay: number;
  Seed: string;
  Weather?: string;
  OnlinePlayers: OnlinePlayers[];
}

export type OnlinePlayers = {
  Name: string;
  Xuid: number;
  Pfid: string;
  AvatarLink: string;
  LocationX: number;
  LocationY: number;
  LocationZ: number;
  LocationDimension: string;
  SpawnTime: string;
}

interface WorldContextType {
  messages: string[];
  lastMessage?: WorldData;
  connection: signalR.HubConnection | null;
}

const WorldContext = createContext<WorldContextType | undefined>(undefined);

export const WorldProvider = ({ children }: { children: React.ReactNode }) => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null
  );
  const [messages, setMessages] = useState<string[]>([]);

  useEffect(() => {
    setConnection(dataHubConnection);
  }, []);

  useEffect(() => {
    if (!connection) return;

    connection
      .start()
      .then(() => {
        console.log("Connected to SignalR Hub");

        connection.on("ReceiveWorldData", (message) => {
          setMessages((prev) => [...prev, message]);
        });

        // Request initial data
        connection.invoke("SendWorldData");

        // Keep updating every 30s
        const interval = setInterval(() => {
          connection.invoke("SendWorldData");
        }, 30 * 1000);

        return () => clearInterval(interval);
      })
      .catch((err) => console.error("Connection failed: ", err));
  }, [connection]);

  const lastMessage =
    messages.length > 0
      ? (JSON.parse(messages[messages.length - 1]) as WorldData)
      : undefined;

  return (
    <WorldContext.Provider value={{ messages, lastMessage, connection }}>
      {children}
    </WorldContext.Provider>
  );
};

// Custom hook for easy usage
// eslint-disable-next-line react-refresh/only-export-components
export const useWorldData = () => {
  const context = useContext(WorldContext);
  if (!context) {
    throw new Error("useWorldData must be used within a WorldProvider");
  }
  return context;
};
