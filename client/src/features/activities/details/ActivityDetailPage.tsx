import { Grid2 } from "@mui/material";
import { useParams } from "react-router";
import { useActivities } from "../../../lib/hooks/useActivities";
import Spinner from "../../../util/Spinner";
import ActivityDetailsHeader from "./ActivityDetailsHeader";
import ActivityDetailsInfo from "./ActivityDetailsInfo";
import ActivityDetailsChat from "./ActivityDetailsChat";
import ActivityDetailsSideBar from "./ActivityDetailsSideBar";



export default function ActivityDetailPage() {
 
    const {id} = useParams();
    const {activity, isLoadingAcivity} = useActivities(id);
     if(isLoadingAcivity) return <Spinner/>
 

  return (

     <Grid2 container spacing={3}>
        <Grid2 size={8}>
            <ActivityDetailsHeader activity={activity as Activity}/>
            <ActivityDetailsInfo activity = {activity as Activity}/>
            <ActivityDetailsChat/>
        </Grid2>
        <Grid2 size={4}>
           <ActivityDetailsSideBar activity={activity as Activity}/>
        </Grid2>
     </Grid2>

  )
}
