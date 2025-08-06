import {
	keepPreviousData,
	useInfiniteQuery,
	useMutation,
	useQuery,
	useQueryClient,
} from "@tanstack/react-query";
import { useAccount } from "./useAccount";
//import { useData } from "./useData";
import agent from "../api/agent";
import { useMemo } from "react";
import { useLocation } from "react-router";
import { useStore } from "./useStore";
import type { FieldValues } from "react-hook-form";

export const useActivities = (id?: string) => {
	const {
		activityStore: { filter, startDate },
	} = useStore();
	const { currentUser } = useAccount();

	const activitiesEndPoint: string = "activities";
	// const {
	// 	items,
	// 	item,
	// 	isLoadingItem,
	// 	isPending,
	// 	updateData,
	// 	createData,
	// 	deleteData,
	// } = useData<Activity>(activitiesEndPoint, id, currentUser);

	const controller = new AbortController();
	const queryClient = useQueryClient();
	const location = useLocation();
	const {
		data: activitiesGroup,
		isPending,
		isFetchingNextPage,
		fetchNextPage,
		hasNextPage,
	} = useInfiniteQuery<PagedList<Activity, string>>({
		queryKey: [activitiesEndPoint, filter, startDate],
		queryFn: async ({ pageParam = null }) => {
			const { data } = await agent.get<PagedList<Activity, string>>(
				`/${activitiesEndPoint}`,
				{
					params: {
						cursor: pageParam,
						pageSize: 3,
						filter,
						startDate,
					},
					signal: controller.signal,
				}
			);
			return data;
		},
		//	staleTime: 1000 * 60 * 5, // 5 minutes stale time
		placeholderData: keepPreviousData,
		initialPageParam: null,
		getNextPageParam: (lastPage) => lastPage.nextCursor,
		// staleTime: 1000 * 60 * 5,    //this is the amount of time the data will be cached option 1
		enabled:
			!id && location.pathname === `/${activitiesEndPoint}` && !!currentUser, //'/activities' // this is another option to only enable when path is activities
		select: (data) => ({
			...data,
			pages: data.pages.map((page) => ({
				...page,
				items: page.items.map((activity) => {
					const host = activity.attendees.find((x) => x.id === activity.hostId);
					return {
						...activity,
						isHost: currentUser?.id === activity.hostId,
						isGoing: activity.attendees.some((x) => x.id === currentUser?.id),
						hostImageUrl: host?.imageUrl,
					};
				}),
			})),
		}),
	});

	const { data: item, isLoading: isLoadingItem } = useQuery({
		queryKey: [activitiesEndPoint, id],
		queryFn: async () => {
			const { data } = await agent.get<Activity>(
				`/${activitiesEndPoint}/${id}`,
				{
					signal: controller.signal,
				}
			);
			return data;
		},
		enabled: !!id && !!currentUser,
	});

	const memoryActivity = useMemo(() => {
		const host = item?.attendees.find((x) => x.id === item.hostId);
		const newItem = {
			...item,
			isHost: currentUser?.id === item?.hostId,
			isGoing: item?.attendees?.some((x) => x.id === currentUser?.id),
			hostImageUrl: host?.imageUrl,
		};
		return newItem;
	}, [item, currentUser]);

	const updateData = useMutation({
		mutationFn: async (activity: FieldValues & { id: string }) => {
			await agent.put(`/${activitiesEndPoint}/${id}`, activity);
		},
		onSuccess: async () => {
			await queryClient.invalidateQueries({
				queryKey: [activitiesEndPoint],
			});
		},
	});

	const createData = useMutation({
		mutationFn: async (activity: FieldValues & { id?: string }) => {
			const { data } = await agent.post<string>(
				`/${activitiesEndPoint}`,
				activity
			);
			return data;
		},
		onSuccess: async () => {
			await queryClient.invalidateQueries({
				queryKey: [activitiesEndPoint],
			});
		},
	});
	const deleteData = useMutation({
		mutationFn: async (id: string) => {
			await agent.delete(`/${activitiesEndPoint}/${id}`);
		},
		onSuccess: async () => {
			await queryClient.invalidateQueries({
				queryKey: [activitiesEndPoint],
			});
		},
	});

	const updateAttendance = useMutation({
		mutationFn: async (id: string) => {
			await agent.post(`/${activitiesEndPoint}/${id}/attend`);
		},
		//optimistic update implementation
		onMutate: async (activityId: string) => {
			await queryClient.cancelQueries({
				queryKey: [activitiesEndPoint, activityId],
			});
			const prevActivity = queryClient.getQueryData<Activity>([
				activitiesEndPoint,
				activityId,
			]);

			queryClient.setQueryData<Activity>(
				[activitiesEndPoint, activityId],
				(oldActivity) => {
					if (!oldActivity || !currentUser) {
						return oldActivity;
					}
					const isHost = oldActivity.hostId === currentUser.id;
					const isAttending = oldActivity.attendees.some(
						(x) => x.id === currentUser.id
					);

					return {
						...oldActivity,
						isCancelled: isHost
							? !oldActivity.isCancelled
							: oldActivity.isCancelled,
						attendees: isAttending
							? isHost
								? oldActivity.attendees
								: oldActivity.attendees.filter((x) => x.id !== currentUser.id)
							: [
									...oldActivity.attendees,
									{
										id: currentUser.id,
										displayName: currentUser.displayName,
										imageUrl: currentUser.imageUrl,
									},
							  ],
					};
				}
			);
			return { prevActivity };
		},
		onError: (error, activityId, context) => {
			console.log(error);
			if (context?.prevActivity) {
				queryClient.setQueryData(
					[activitiesEndPoint, activityId],
					context.prevActivity
				);
			}
		},
	});

	return {
		activitiesGroup: activitiesGroup,
		isFetchingNextPage,
		fetchNextPage,
		hasNextPage,
		activity: memoryActivity,
		isPending: isPending,
		isLoadingAcivity: isLoadingItem,
		updateActivity: updateData,
		createActivity: createData,
		deleteActivity: deleteData,
		updateAttendance: updateAttendance,
	};
};
