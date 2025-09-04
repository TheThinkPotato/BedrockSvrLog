import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { ApiUrl, headers } from "../Api/Api";

export type UserPlayerKills = {
  xuid: number;
  name: string;
  totalKills: number;
  favouriteKillEntity: string;
  favouriteKillCount: number;

  playerKillsList: PlayerKillsList[];
};

export type PlayerKillsList = {
  entityType: string;
  killCount: number;
};

const fetchUserPlayerKills = async (xuid: number) => {
  const { data } = await axios.get<UserPlayerKills>(
    `${ApiUrl}/user/${xuid}/PlayerKills`,
    {
      headers: headers,
    }
  );
  return data;
};

export const useGetUserPlayerKills = (xuid: number, enabled = true) => {
  const { data, isLoading, error } = useQuery({
    queryKey: ["userPlayerKills", xuid],
    queryFn: () => fetchUserPlayerKills(xuid),
    staleTime: 1000 * 60 * 1, // 1 minute
    enabled: enabled,
  });

  return {
    data: data,
    isLoading,
    error,
  };
};
