import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent";
import { useMemo } from "react";

const profileEndpoint = "profiles";
const photosEndpoint = "photos";
export const useProfile = (id?: string) => {
	const queryClient = useQueryClient();
	const { data: profile, isLoading: loadingProfile } = useQuery<Profile>({
		queryKey: [profileEndpoint, id],
		queryFn: async () => {
			const response = await agent.get<Profile>(`/${profileEndpoint}/${id}`);
			return response.data;
		},
		enabled: !!id,
	});

	const { data: photos, isLoading: loadingPhotos } = useQuery<Photo[]>({
		queryKey: [photosEndpoint, id],
		queryFn: async () => {
			const response = await agent.get<Photo[]>(
				`/${profileEndpoint}/${id}/${photosEndpoint}`
			);
			return response.data;
		},
		enabled: !!id,
	});

	const uploadPhoto = useMutation({
		mutationFn: async (formFile: Blob) => {
			const formData = new FormData();
			formData.append("formFile", formFile);
			const response = await agent.post(
				`/${profileEndpoint}/add-photo`,
				formData,
				{
					headers: { "Content-Type": "multipart/form-data" },
				}
			);
			return response.data;
		},
		onSuccess: async (photo: Photo) => {
			await queryClient.invalidateQueries({
				queryKey: [`${photosEndpoint}`, id],
			});
			queryClient.setQueryData(["user"], (data: User) => {
				if (!data) return data;
				return {
					...data,
					imageUrl: data.imageUrl ?? photo.url,
				};
			});
			queryClient.setQueryData([`${profileEndpoint}`, id], (data: Profile) => {
				if (!data) return data;
				return {
					...data,
					imageUrl: data.imageUrl ?? photo.url,
				};
			});
		},
	});

	const setMainPhoto = useMutation({
		mutationFn: async (photo: Photo) => {
			await agent.put(`/${profileEndpoint}/${photo.id}/setmain`);
		},
		onSuccess: (_, photo) => {
			queryClient.setQueryData(["user"], (userData: User) => {
				if (!userData) return userData;
				return {
					...userData,
					imageUrl: photo.url,
				};
			});
			queryClient.setQueryData(
				[`${profileEndpoint}`, id],
				(profileData: Profile) => {
					if (!profileData) return profileData;
					return {
						...profileData,
						imageUrl: photo.url,
					};
				}
			);
		},
	});

	const deletePhoto = useMutation({
		mutationFn: async (photoId: string) => {
			await agent.delete(`/${profileEndpoint}/${photoId}/${photosEndpoint}`);
		},
		onSuccess: (_, photoId) => {
			queryClient.setQueryData(
				[`${photosEndpoint}`, id],
				(photosData: Photo[]) => {
					return photosData?.filter((photo) => photo.id !== photoId);
				}
			);
		},
	});

	const isCurrentUser = useMemo(() => {
		return id === queryClient.getQueryData<User>(["user"])?.id;
	}, [id, queryClient]);

	return {
		profile,
		loadingProfile,
		photos,
		loadingPhotos,
		isCurrentUser,
		uploadPhoto,
		setMainPhoto,
		deletePhoto,
	};
};
