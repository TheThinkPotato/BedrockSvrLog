import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { ApiUrl, headers } from "../Api/Api";

export type User = {
  xuid: number;
  name: string;
  pfid: string;
  diceBearAvatarUrl: string;
};

const fetchUsers = async () => {
  const { data } = await axios.get<User[]>(`${ApiUrl}/users`, { headers });
  return data;
};

const useGetUsers = () => {
  const { data, isLoading, error } = useQuery({
    queryKey: ["users"],
    queryFn: fetchUsers,
    staleTime: 1000 * 60 * 5, // 5 minutes
  });

  return {
    data,
    isLoading,
    error,
  };
};

export default useGetUsers;
