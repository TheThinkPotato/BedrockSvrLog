import { useQuery } from "@tanstack/react-query"
import { ApiUrl, headers } from "../Api/Api";
import axios from "axios";

export type World = {
    name: string;
    seed: string;
};

const fetchWorld = async () => {
    const { data } = await axios.get<World>(
        `${ApiUrl}/world`,
        {
            headers: headers,
        }
    );
    return data;
};

const useGetWorld = () => {
    const { data, isLoading, error } = useQuery({
        queryKey: ["world"],
        queryFn: fetchWorld,
        staleTime: 1000 * 60 * 60 * 2, // 2 hours
        enabled: true,
    });

    return {
        data: data,
        isLoading,
        error,
    };
};

export default useGetWorld;