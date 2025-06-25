
import { useState } from "react";

import { Box, Container, CssBaseline } from "@mui/material";
import NavBar from "./NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import Spinner from "../../util/Spinner"
import { useActivities } from "../../lib/types/hooks/useActivities";


function App() {


  const [selectedActivity, setSelectedActivity] = useState<Activity | undefined>(undefined);
  const [editMode, setEditMode] = useState(false);


 const { activities, isPending } = useActivities();


  const handleSelectActivity = (id: string) => {
    setSelectedActivity(activities!.find(activity => activity.id === id));
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



  return (
    <>
    <CssBaseline />
    <NavBar openForm={handleOpenForm} />
   
     <Box sx={{bgcolor:'#eeeeee', minHeight:'100vh'}}>
       <Container maxWidth='xl' sx={{mt:3}}>
         {!activities || isPending ? ( 
            <Spinner/>
              ) : (
                    <ActivityDashboard 
                      activities={activities}
                      selectActivity={handleSelectActivity}
                      cancelSelectedActivity ={handleCancelSelectActivity} 
                      selectedActivity ={selectedActivity}
                      editMode ={editMode}
                      openForm ={handleOpenForm}
                      closeForm={handleFormClose}
                    />
                  )
          }       
       </Container>
     </Box>
    </>
  )
}

export default App
