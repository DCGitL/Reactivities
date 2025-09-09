import { useQuery } from "@tanstack/react-query";
import agent from "../api/agent";
import { useState } from "react";

export const useWeather = (lat: number, lon: number) => {
	const [getWeather, setGetWeather] = useState<boolean>(false);

	const {
		data: weather,
		isLoading: weatherIsLoading,
		error: weatherError,
	} = useQuery({
		queryKey: ["weather", lat, lon],
		queryFn: async () => {
			const response = await agent.get<weatherResponse>(
				`/activities/${lat}/${lon}/weather`
			);
			return response.data;
		},
		enabled: getWeather && lat != 0 && lon != 0,
		staleTime: 1000 * 3600 * 24, //1day
	});

	return { weather, weatherIsLoading, weatherError, setGetWeather };
};
