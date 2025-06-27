import axios from "axios";
import Config from "../../util/Config";

const sleep = (delay: number)=> {
    return new Promise((resolve) =>{
        setTimeout(resolve, delay);
    })

}
const agent = axios.create({
  baseURL: Config.userBaseURL,
});

agent.interceptors.response.use( async (response) => {
    try{
        console.log('interceptor', response);
        await sleep(1000); // Simulate a delay for all requests
        return response;

    }
    catch(error){
        console.log(error);
        return Promise.reject(error);
    }
});

export default agent;