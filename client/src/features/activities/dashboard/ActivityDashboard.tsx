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

}

//rfc 
export default function ActivityDashboard({
  activities,
  selectedActivity, 
  selectActivity, 
  cancelSelectedActivity,
  openForm,
  closeForm,
  editMode 
}: Props ) {
  return (
   <Grid2 container spacing ={2}>
      <Grid2 size={7}>
        <ActivityList 
         activities={activities}
         handleViewClick={selectActivity}/> 
      </Grid2>
      <Grid2  size={5}>
        {selectedActivity && !editMode && 
        <ActivityDetail 
          selectedActivity={selectedActivity}  
          cancelSelectActivity={cancelSelectedActivity}
          openForm={openForm} />}  
         {editMode && <ActivityForm 
         activity={selectedActivity} closeForm={closeForm} />}  
      </Grid2>
    </Grid2>
  ) 
}
