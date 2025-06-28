import axios from "axios";
import Config from "../../util/Config";
import { store } from "../stores/store";

const sleep = (delay: number)=> {
    return new Promise((resolve) =>{
        setTimeout(resolve, delay);
    })

}
const agent = axios.create({
  baseURL: Config.userBaseURL,
});
agent.interceptors.request.use(config =>{
    store.uiStore.isBusy();
    return config;
})

agent.interceptors.response.use( async (response) => {
    try{
        await sleep(1000); // Simulate a delay for all requests
        return response;

    }
    catch(error){
        console.log(error);
        return Promise.reject(error);
    }
    finally{
        store.uiStore.isIdle();
    }
});

export default agent;