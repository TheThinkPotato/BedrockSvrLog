import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { API, headers } from "../Api/Api";

export interface RealmEvent {
  xuid: number;
  name: string;
  pfid: string;
  realmEvent: string;
  eventTime: string;
  diceBearAvatarUrl: string;
}

export interface RealmEventResponse {
  users: RealmEvent[];
}

const fetchRealmEvents = async () => {
  const { data } = await axios.get<RealmEventResponse>(`${API}/realmevents`, {
    headers: headers,
  });
  return data;
};

export const useGetRealmEvents = () => {
  const { data, isLoading, error } = useQuery({
    queryKey: ["realmEvents"],
    queryFn: fetchRealmEvents,
    staleTime: 1000 * 60 * 5, // 5 minutes
  });

  return {
    data: data,
    isLoading,
    error,
  };
};
