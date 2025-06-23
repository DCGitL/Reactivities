import { List, ListItem, ListItemText, Typography } from "@mui/material";
import axios from "axios";
import { useEffect, useState } from "react";

function App() {

  const [activities, setActivities] = useState<Activity[]>([]);
  
  useEffect(() => {
    const controller = new AbortController();
axios.get<Activity[]>("https://localhost:5001/api/activities",  { signal: controller.signal })
    .then(({data}) => {
      setActivities(data);
    })
    .catch((error) => {
      console.error("Error fetching activities:", error);
    });

    return () => {
      controller.abort();
    };

  }, []);
  return (
    <>
     <Typography variant="h3">Reactivities</Typography>
     <List>
        {activities.map((activity) => (
          <ListItem key={activity.id}>    
             <ListItemText>{activity.title} - {activity.date}</ListItemText> 
          </ListItem>
        ))}
      </List>
    </>
  )
}

export default App
