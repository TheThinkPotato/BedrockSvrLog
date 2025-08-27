import * as signalR from "@microsoft/signalr";

export const API = import.meta.env.VITE_API || "http://localhost:5000";

export const ApiUrl = `${API}/API`;
export const DataHubUrl = `${API}/DataHub`;

export const headers = {
  "Content-Type": "application/json",
  "Access-Control-Allow-Origin": "*",
  "Access-Control-Allow-Methods": "GET, POST, PUT, DELETE, OPTIONS",
  "Access-Control-Allow-Headers": "Content-Type, Authorization",
};

export const dataHubConnection = new signalR.HubConnectionBuilder()
  .withUrl(DataHubUrl) // C# SignalR URL
  .withAutomaticReconnect()
  .build();
