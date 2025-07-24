import { useLocalObservable } from "mobx-react-lite";
import {
	HubConnection,
	HubConnectionBuilder,
	HubConnectionState,
} from "@microsoft/signalr";
import Config from "../../util/Config";
import { useEffect, useRef } from "react";
import { runInAction } from "mobx";

export const useComments = (activityId?: string) => {
	const created = useRef(false);
	const commentApiUrl = Config.userCommentBaseURL;
	const commentStore = useLocalObservable(() => ({
		comments: [] as ChatComment[],
		hubConnection: null as HubConnection | null,

		createHubConnection(activityId: string) {
			if (!activityId) return;

			this.hubConnection = new HubConnectionBuilder()
				.withUrl(`${commentApiUrl}?activityId=${activityId}`, {
					withCredentials: true,
				})
				.withAutomaticReconnect()
				.build();
			this.hubConnection
				.start()
				.catch((error) =>
					console.error("Error establishing connection: ", error)
				);

			this.hubConnection.on("LoadComments", (comments: ChatComment[]) => {
				runInAction(() => {
					this.comments = comments;
					console.log("Comments loaded:", this.comments);
				});
			});
			this.hubConnection.on("ReceiveComment", (comment: ChatComment) => {
				runInAction(() => {
					this.comments.unshift(comment); // Add new comment to the top
					console.log("Comment received:", comment);
				});
			});
		},
		stopHubConnection() {
			if (this.hubConnection?.state === HubConnectionState.Connected) {
				this.hubConnection
					.stop()
					.catch((error) =>
						console.error("Error stopping connection: ", error)
					);
			}
		},
	}));

	useEffect(() => {
		if (activityId && !created.current) {
			commentStore.createHubConnection(activityId);
			created.current = true;
		}
		return () => {
			commentStore.stopHubConnection();
			commentStore.comments = [];
		};
	}, [activityId, commentStore]);

	return {
		commentStore,
	};
};
