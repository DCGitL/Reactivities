import { Box } from '@mui/material'

import ActivityCard from './ActivityCard';

type Props= {
    activities: Activity[];
    handleViewClick: (id: string) => void;
 
}
export default function ActivityList({activities,handleViewClick }: Props) {


  return (
   <Box sx={{display: 'flex', flexDirection: 'column', gap: 3}}>
        {activities.map((activity) => ( 
          <ActivityCard  
          handleSelectedViewClick = {()=> handleViewClick(activity.id)} key={activity.id} activity={activity}
         
          />))}
        
   </Box>
  )
}
