import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { ApiUrl, headers } from "../Api/Api";

export interface Duration {
  xuid: number;
  name: string;
  pfid: string;
  diceBearAvatarUrl: string;
  isOnline: boolean;
  lastLogin: string;
  spawnTime: string;
  timeSinceLastLogin: string;
  timeSinceLastSpawn: string;
  totalDuration: string;
  totalGameplayDuration: string;
  totalLiveDuration: string;
  totalLiveGameplayDuration: string;
}

export interface DurationResponse {
  durations: Duration[];
}

const fetchDurations = async () => {
  const { data } = await axios.get<DurationResponse>(`${ApiUrl}/durations`, {
    headers: headers,
  });
  return data;
};

export const useGetDurations = (enabled = true) => {

  const { data, isLoading, error } = useQuery({
    queryKey: ["durations"],
    queryFn: fetchDurations,
    staleTime: 1000 * 60 * 5, // 5 minutes
    enabled: enabled,
  });

  return {
    data: data,
    isLoading,
    error,
  };
};
