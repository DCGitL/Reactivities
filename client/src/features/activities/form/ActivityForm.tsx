import { Box, Button, Paper, TextField, Typography } from '@mui/material'


type Props = {
  closeForm : () => void  // onSubmit: (data: any) => void;
  activity?: Activity; // onCancel: () => void;
  submitForm: (activity: Activity) => void; // onChange: (data: any) => void;
}

export default function ActivityForm({closeForm, activity, submitForm}: Props) {
    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
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
        if (activity) data.id = activity.id; // If editing, include the existing ID
        submitForm(data as unknown as Activity);
            
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
            <TextField name ='date'  label='Date' type='date'  defaultValue={activity?.date} />
            <TextField name ='city' label='City'  defaultValue={activity?.city} />
            <TextField name ='venue' label='Venue'  defaultValue={activity?.venue} />
            <Box display={'flex'} justifyContent='end' gap={3}>
                <Button onClick={closeForm} color='inherit' >Cancel</Button>
                <Button type='submit' color='success' variant='contained'>Submit</Button>
            </Box>
            
        </Box>
    </Paper>
  )
}
