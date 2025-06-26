import { Box, Button, Card, CardActions, CardContent, Chip, Typography } from "@mui/material"
import { useActivities } from "../../../lib/types/hooks/useActivities";
import { Link } from "react-router";

type Props ={
  activity: Activity;
}

export default function ActivityCard({activity}: Props) {
 const {deleteActivity} = useActivities();

  const handleOnDelete = async (id: string) => {
    
    await deleteActivity.mutateAsync(id);

  }
 
  return (
    <Card sx={{borderRadius: 3}}>
        <CardContent>
            <Typography variant="h5">{activity.title}</Typography>
            <Typography sx={{color: 'text.secondary',mb: 1}}>{activity.date}</Typography>
            <Typography variant="body2">{activity.description}</Typography>
            <Typography variant='subtitle1'>{activity.city} / {activity.venue}</Typography>
        </CardContent>
        <CardActions sx={{display: 'flex', justifyContent: 'space-between', pb:2}}>
           <Chip label={activity.category} variant='outlined'/>
           <Box sx={{display: 'flex', gap: 1}}>
             <Button size='medium' 
                  component={Link} to={`/activities/${activity.id}`} 
                  variant='contained'>View</Button>
             <Button size='medium' 
                     onClick={() => handleOnDelete(activity.id)} 
                     variant='outlined' 
                     color='error'
                     disabled={deleteActivity.isPending}>
                         Delete
                      </Button>
           </Box>
        </CardActions>
    </Card>
  )
}
