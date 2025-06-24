import { Grid2 } from '@mui/material'
import ActivityList from './ActivityList';
import ActivityDetail from '../details/ActivityDetail';
import ActivityForm from '../form/ActivityForm';

type Props = {
    activities: Activity[];
    selectActivity: (id: string) => void;
    cancelSelectedActivity: () => void;
    selectedActivity?: Activity ;
    editMode: boolean;
    openForm: (id: string) => void;
    closeForm: () => void;
    submitForm: (activity: Activity) => void;
    deleteActivity: (id: string) => void;
}

//rfc 
export default function ActivityDashboard({
  activities,
  selectedActivity, 
  selectActivity, 
  cancelSelectedActivity,
  openForm,
  closeForm,
  editMode, 
  submitForm,
  deleteActivity
}: Props ) {
  return (
   <Grid2 container spacing ={2}>
      <Grid2 size={7}>
        <ActivityList 
         activities={activities}
         handleViewClick={selectActivity}
         onDeleleteActivity={deleteActivity}/> 
      </Grid2>
      <Grid2  size={5}>
        {selectedActivity && !editMode && 
        <ActivityDetail 
          activity={selectedActivity}  
          cancelSelectActivity={cancelSelectedActivity}
          openForm={openForm} />}  
         {editMode && <ActivityForm submitForm={submitForm} activity={selectedActivity} closeForm={closeForm} />}  
      </Grid2>
    </Grid2>
  ) 
}
