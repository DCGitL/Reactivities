import { Box, Button, Paper, TextField, Typography } from '@mui/material'
import { useActivities } from '../../../lib/types/hooks/useActivities';


type Props = {
  closeForm : () => void  // onSubmit: (data: any) => void;
  activity?: Activity; // onCancel: () => void;

}

export default function ActivityForm({closeForm, activity}: Props) {

     const { updateActivity, createActivity } = useActivities();

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);
        const data: {[key: string]: FormDataEntryValue } = {};
        formData.forEach((value, key) => {
            data[key] = value;
        });
        // = {
        //     title: formData.get('title') as string,
        //     category: formData.get('category') as string,
        //     description: formData.get('description') as string,
        //     date: formData.get('date') as string,
        //     city: formData.get('city') as string,
        //     venue: formData.get('venue') as string
        // };  
        if (activity){
            data.id = activity.id;
           await  updateActivity.mutateAsync(data as unknown as Activity);
           closeForm();
           console.log('Activity updated:', data);
        }  // If editing, include the existing ID
        else {
            await createActivity.mutateAsync(data as unknown as Activity);
            closeForm();
        }
            
    }
  return (
    <Paper sx={{borderRadius: 3, padding: 3}}>
        <Typography variant='h5' gutterBottom color='primary'>
            Create activity
        </Typography>
        <Box onSubmit={handleSubmit} component={'form'} sx={{display: 'flex', flexDirection: 'column', gap: 3}}>
            <TextField name ='title' label='Title' defaultValue={activity?.title}/>
            <TextField name ='category'  label='Category'  defaultValue={activity?.category} />
            <TextField name ='description'  label='Description' multiline rows={3}  defaultValue={activity?.description}/>
            <TextField name ='date'  label='Date' type='date'
              defaultValue={
                activity?.date ? new Date(activity.date).toISOString().split('T')[0]
                : new Date().toISOString().split('T')[0]    
                } />
            <TextField name ='city' label='City'  defaultValue={activity?.city} />
            <TextField name ='venue' label='Venue'  defaultValue={activity?.venue} />
            <Box display={'flex'} justifyContent='end' gap={3}>
                <Button onClick={closeForm} color='inherit' >Cancel</Button>
                <Button 
                  type='submit'
                  color='success' 
                  variant='contained'
                  disabled ={updateActivity.isPending || createActivity.isPending}>Submit</Button>
            </Box>
            
        </Box>
    </Paper>
  )
}
