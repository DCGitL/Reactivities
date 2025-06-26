import { Box } from '@mui/material'

import ActivityCard from './ActivityCard';
import { useActivities } from '../../../lib/types/hooks/useActivities';
import Spinner from '../../../util/Spinner';


export default function ActivityList() {
  const {activities, isPending} = useActivities();
  if(!activities || isPending) return <Spinner/>   // <Typography>Loading..</Typography>


  return (
   <Box sx={{display: 'flex', flexDirection: 'column', gap: 3}}>
        {activities.map((activity) => ( 
          <ActivityCard  key={activity.id} activity={activity}    
          />))}
        
   </Box>
  )
}
