import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { LoginSchema } from "../schemas/loginSchema";
import agent from "../api/agent";
import { useNavigate } from "react-router";
import type { RegisterSchema } from "../schemas/registerSchema";
import { toast } from "react-toastify";
import { useState } from "react";

export const useAccount = () => {
	const queryClient = useQueryClient();
	const navigate = useNavigate();
	const [requestUserInfo, setRequestUserInfo] = useState<boolean>(false);

	const { data: currentUser, isLoading: loadingUserInfo } = useQuery({
		queryKey: ["user"],
		queryFn: async () => {
			const response = await agent.get<User>("/account/user-info");
			return response.data;
		},
		enabled: requestUserInfo,
		staleTime: 1000 * 5,

		///cache the data instead of making a server call
	});

	const loginUser = useMutation({
		mutationFn: async (creds: LoginSchema) => {
			const result = await agent.post<User>(
				"/account/login?useCookies=true",
				creds
			);
			return result.data;
		},
		onSuccess: async (data) => {
			setRequestUserInfo(true);
			queryClient.setQueryData(["user"], (olduser: User) => {
				const currentUserLogedIn = olduser ?? {};
				return { ...currentUserLogedIn, ...data };
			});
		},
		onError: () => {
			setRequestUserInfo(false);
		},
	});

	const registerUser = useMutation({
		mutationFn: async (cred: RegisterSchema) => {
			await agent.post("/account/register", cred);
		},
		onSuccess: async () => {
			toast.success("Register succeccful - you can now login");
			await queryClient.invalidateQueries({
				queryKey: ["user"],
			});
			navigate("/login");
		},
	});

	const logoutUser = useMutation({
		mutationFn: async () => {
			await agent.post("/account/logout");
		},
		onSuccess: () => {
			queryClient.removeQueries({ queryKey: ["user"] });
			queryClient.removeQueries({ queryKey: ["activities"] });
			navigate("/");
		},
	});

	return {
		loginUser,
		currentUser,
		logoutUser,
		loadingUserInfo,
		registerUser,
	};
};
