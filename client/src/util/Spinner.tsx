import { CircularProgress, Stack } from '@mui/material'


export default function Spinner() {
  return (
    <Stack sx={{ color: 'grey.500', justifyContent:'center' }} spacing={2} direction="row">
      <CircularProgress color="secondary" />
      <CircularProgress color="success" />
      <CircularProgress color="inherit" />
    </Stack>
  )
}
