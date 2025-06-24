import { Box, Container, CssBaseline } from "@mui/material";
import axios, { CanceledError } from "axios";
import { useEffect, useState } from "react";
import NavBar from "./NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";

function App() {

  const [activities, setActivities] = useState<Activity[]>([]);
  const [selectedActivity, setSelectedActivity] = useState<Activity | undefined>(undefined);
  const [editMode, setEditMode] = useState(false);

  useEffect(() => {
    const controller = new AbortController();
axios.get<Activity[]>("https://localhost:5001/api/activities",  { signal: controller.signal })
    .then(({data}) => {
      setActivities(data);
    })
    .catch((error) => {
      if(error instanceof CanceledError) 
        return;
      console.error("Error fetching activities:", error);
    });

    return () => {
      controller.abort();
    };

  }, []);

  const handleSelectActivity = (id: string) => {
    setSelectedActivity(activities.find(activity => activity.id === id));
  };  
  const handleCancelSelectActivity = () => {
    setSelectedActivity(undefined);
  };

  const handleOpenForm = (id?: string) => {
    if (id) handleSelectActivity(id);
    else handleCancelSelectActivity();
    setEditMode(true);
  }

  const handleFormClose = () => {
    setEditMode(false);
  }

  const handleSubmit = (activity: Activity) => {
    if (activity.id) {
      setActivities(activities.map(a => a.id === activity.id ? activity : a));

    }
    else {
      const newActivity = {...activity, id: (activities.length).toString()}; // Generate a new ID
      setActivities([...activities, newActivity]);
      setSelectedActivity(newActivity);
      console.log('New activity :', newActivity );
    }
    setEditMode(false);
    console.log('Submitted activity:', activity);
  };

  const handleDelete = (id:string )=>{
    setActivities(activities.filter(activity => activity.id !== id));
   
    console.log('Deleted activity with id:', id);
    handleCancelSelectActivity();
  }
  return (
    <>
    <CssBaseline />
    <NavBar openForm={handleOpenForm} />
   
     <Box sx={{bgcolor:'#eeeeee'}}>
       <Container maxWidth='xl' sx={{mt:3}}>
         <ActivityDashboard 
          activities={activities}
          selectActivity={handleSelectActivity}
          cancelSelectedActivity ={handleCancelSelectActivity} 
          selectedActivity ={selectedActivity}
          editMode ={editMode}
          openForm ={handleOpenForm}
          closeForm={handleFormClose}
          submitForm={handleSubmit}
          deleteActivity={handleDelete}
          />
       </Container>
     </Box>
    </>
  )
}

export default App
