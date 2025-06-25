
import { useData } from "./useData";

export  const useActivities = () => {

   const { data, isPending, updateData, createData, deleteData } = useData<Activity>('activities');

  const activities = data,  
        updateActivity = updateData, 
        createActivity = createData, 
        deleteActivity = deleteData;  

    return { activities, isPending, updateActivity, createActivity, deleteActivity};
  };

  