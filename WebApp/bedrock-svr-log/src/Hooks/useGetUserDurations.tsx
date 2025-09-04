import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { ApiUrl, headers } from "../Api/Api";

export type UserDuration = {
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
  lastLogOut: string;
}


const fetchUserDurations = async (xuid: number) => {
  const { data } = await axios.get<UserDuration>(`${ApiUrl}/user/${xuid}/userDurations`, {
    headers: headers,
  });
  return data;
};

export const useGetUserDurations = (xuid: number, enabled = true) => {

  const { data, isLoading, error } = useQuery({
    queryKey: ["userDurations", xuid],
    queryFn: () => fetchUserDurations(xuid),
    staleTime: 1000 * 60 * 1, // 1 minute
    enabled: enabled,
  });

  return {
    data: data,
    isLoading,
    error,
  };
};
