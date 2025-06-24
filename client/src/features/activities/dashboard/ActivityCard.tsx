import { Box, Button, Card, CardActions, CardContent, Chip, Typography } from "@mui/material"

type Props ={
    activity: Activity
    handleSelectedViewClick: (id: string) => void;
    onDelete: (id: string) => void;
}

export default function ActivityCard({activity, handleSelectedViewClick,onDelete}: Props) {
 
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
             <Button size='medium' onClick={() => handleSelectedViewClick(activity.id) } variant='contained'>View</Button>
             <Button size='medium' onClick={() => onDelete(activity.id)} variant='outlined' color='error'>Delete</Button>
           </Box>
        </CardActions>
    </Card>
  )
}
