import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { ApiUrl, headers } from "../Api/Api";
import type { RealmEvent } from "./useGetRealmEvents";

export type UserRealmEvent = {
    realmEvents: RealmEvent[];
    hasRealmEvents: boolean;
}

const fetchUserRealmEvents = async (xuid: number) => {
  const { data } = await axios.get<UserRealmEvent>(
    `${ApiUrl}/user/${xuid}/realmEvents`,
    {
      headers: headers,
    }
  );
  return data;
};

export const useGetUserRealmEvents = (xuid: number, enabled = true) => {
  const { data, isLoading, error } = useQuery({
    queryKey: ["userRealmEvents", xuid],
    queryFn: () => fetchUserRealmEvents(xuid),
    staleTime: 1000 * 60 * 5, // 5 minutes
    enabled: enabled,
  });

  return {
    data: data,
    isLoading,
    error,
  };
};
