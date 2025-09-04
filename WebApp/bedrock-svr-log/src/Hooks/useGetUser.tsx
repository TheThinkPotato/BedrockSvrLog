import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { ApiUrl, headers } from "../Api/Api";

export interface User {
  xuid: number;
  name: string;
  pfid: string;
  diceBearAvatarUrl: string;
}

const fetchUser = async (xuid: number) => {
    console.log("xuid>>", xuid);
  const { data } = await axios.get<User>(
    `${ApiUrl}/user/${xuid}`,
    {
      headers: headers,
    }
  );
  return data;
};

export const useGetUser = (xuid: number, enabled = true) => {
  const { data, isLoading, error } = useQuery({
    queryKey: ["user", xuid],
    queryFn: () => fetchUser(xuid),
    staleTime: 1000 * 60 * 1, // 1 minute
    enabled: enabled,
  });

  return {
    data: data,
    isLoading,
    error,
  };
};
