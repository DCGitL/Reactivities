import { Grid2, Typography } from "@mui/material";
import { useParams } from "react-router";
import { useActivities } from "../../../lib/types/hooks/useActivities";
import Spinner from "../../../util/Spinner";
import ActivityDetailsHeader from "./ActivityDetailsHeader";
import ActivityDetailsInfo from "./ActivityDetailsInfo";
import ActivityDetailsChat from "./ActivityDetailsChat";
import ActivityDetailsSideBar from "./ActivityDetailsSideBar";



export default function ActivityDetailPage() {
 
    const {id} = useParams();
    const {activity, isLoadingAcivity} = useActivities(id);
     if(isLoadingAcivity) return <Spinner/>
    if (!activity) return <Typography>Activity not found</Typography>; // Ensure the activity exists before rendering

  return (

     <Grid2 container spacing={3}>
        <Grid2 size={8}>
            <ActivityDetailsHeader activity={activity}/>
            <ActivityDetailsInfo activity = {activity}/>
            <ActivityDetailsChat/>
        </Grid2>
        <Grid2 size={4}>
           <ActivityDetailsSideBar/>
        </Grid2>
     </Grid2>

  )
}
