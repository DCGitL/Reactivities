import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useAccount } from "./useAccount";
import { useData } from "./useData";
import agent from "../api/agent";
import { useMemo } from "react";

export const useActivities = (id?: string | number) => {
	const { currentUser } = useAccount();
	const queryClient = useQueryClient();
	const activitiesEndPoint: string = "activities";
	const {
		items,
		item,
		isLoadingItem,
		isPending,
		updateData,
		createData,
		deleteData,
	} = useData<Activity>(activitiesEndPoint, id, currentUser);
	const newItems = items?.map((activity) => {
		const host = activity.attendees.find((x) => x.id === activity.hostId);
		return {
			...activity,
			isHost: currentUser?.id === activity.hostId,
			isGoing: activity.attendees.some((x) => x.id === currentUser?.id),
			hostImageUrl: host?.imageUrl,
		};
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
					// console.log('setQueryData old',oldActivity);
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
		activities: newItems,
		activity: memoryActivity,
		isPending: isPending,
		isLoadingAcivity: isLoadingItem,
		updateActivity: updateData,
		createActivity: createData,
		deleteActivity: deleteData,
		updateAttendance: updateAttendance,
	};
};
