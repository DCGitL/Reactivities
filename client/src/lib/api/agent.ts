import axios from "axios";
import Config from "../../util/Config";
import { store } from "../stores/store";
import { toast } from "react-toastify";
import { router } from "../../app/layout/router/Routes";

const sleep = (delay: number) => {
	return new Promise((resolve) => {
		setTimeout(resolve, delay);
	});
};
const agent = axios.create({
	baseURL: Config.userBaseURL,
	withCredentials: true,
});
agent.interceptors.request.use((config) => {
	store.uiStore.isBusy();
	return config;
});

agent.interceptors.response.use(
	async (response) => {
		if (import.meta.env.DEV) await sleep(1000); // Simulate a delay for all requests
		store.uiStore.isIdle();
		return response;
	},
	async (error) => {
		if (import.meta.env.DEV) await sleep(1000);
		store.uiStore.isIdle();
		const { status, data } = error.response;
		switch (status) {
			case 400:
				// toast.error('bad request');
				if (data.errors) {
					const modalStateErrors = [];
					for (const key in data.errors) {
						if (data.errors[key]) {
							modalStateErrors.push(data.errors[key]);
						}
					}
					throw modalStateErrors.flat();
				} else {
					toast.error(data);
				}
				break;
			case 401:
				if (data?.detail === "NotAllowed") {
					throw new Error(data.detail);
				} else {
					const errormessage = data?.detail;
					toast.error("Unauthorized " + errormessage);
				}
				break;
			case 404:
				router.navigate("/not-found");
				break;
			case 500:
				router.navigate("/server-error", { state: { error: data } });
				break;
			default:
				break;
		}
		return Promise.reject(error);

		//rethrow the error for react query to handle the error
	}
);

export default agent;
