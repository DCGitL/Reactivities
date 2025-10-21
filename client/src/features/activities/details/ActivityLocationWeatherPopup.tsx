import * as React from "react";
import Button from "@mui/material/Button";
import { styled } from "@mui/material/styles";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import CloseIcon from "@mui/icons-material/Close";
import Typography from "@mui/material/Typography";
import { useWeather } from "../../../lib/hooks/useWeather";
import WeatherCard from "../../weather/WeatherCard";
import { Box, Grid2 } from "@mui/material";
import { formatDate } from "../../../util/DateFormater";

const BootstrapDialog = styled(Dialog)(({ theme }) => ({
	"& .MuiDialogContent-root": {
		padding: theme.spacing(2),
	},
	"& .MuiDialogActions-root": {
		padding: theme.spacing(1),
	},
}));
type Props = {
	long: number;
	lat: number;
};

export default function ActivityLocationWeatherPopup({ long, lat }: Props) {
	const { weather, weatherIsLoading, setGetWeather, weatherError } = useWeather(
		lat,
		long
	);
	const [open, setOpen] = React.useState(false);

	const handleClickOpen = () => {
		setGetWeather(true);
		setOpen(true);
	};
	const handleClose = () => {
		setOpen(false);
	};

	if (weatherIsLoading) return <Typography>Loading.....</Typography>;
	if (weatherError) {
		setGetWeather(false);
		return (
			<Typography color="error">
				Error loading weather data: {weatherError.message}
			</Typography>
		);
	}

	return (
		<React.Fragment>
			<Button onClick={handleClickOpen}>Get Weather for the location</Button>
			<BootstrapDialog
				onClose={handleClose}
				fullWidth
				sx={{ borderRadius: 10 }}
				aria-labelledby="customized-dialog-title"
				open={open}>
				<DialogTitle
					sx={{ m: 0, p: 2 }}
					id="customized-dialog-title">
					<Typography variant="h6">
						Weather Report Country: {weather?.sys.country}
					</Typography>
					<Typography variant="h6">City: {weather?.city}</Typography>
				</DialogTitle>
				<IconButton
					aria-label="close"
					onClick={handleClose}
					sx={(theme) => ({
						position: "absolute",
						right: 8,
						top: 8,
						color: theme.palette.grey[500],
					})}>
					<CloseIcon />
				</IconButton>
				<DialogContent dividers>
					{weather && (
						<Box
							sx={{
								display: "flex",
								flexDirection: "column",
								alignItems: "start",
								gap: 2,
							}}>
							<Typography>
								Eastern Date/time: {formatDate(weather.dt)}
							</Typography>
							<Typography>
								{weather.city}- Local Time: {weather.localDateTime}
							</Typography>
						</Box>
					)}
					<Box
						sx={{
							display: "flex",
							flexDirection: "row",
							alignItems: "center",
							gap: 5,
							mt: 2,
						}}>
						<Typography
							variant="body1"
							gutterBottom>
							Sunrise at: {weather?.sys.sunrise.toString()}
						</Typography>
						<Typography
							variant="body1"
							gutterBottom>
							Sunset at: {weather?.sys.sunset}
						</Typography>
					</Box>
				</DialogContent>
				<DialogContent dividers>
					{weather && weather.weather.length > 0 ? (
						<WeatherCard weather={weather.weather} />
					) : (
						<Typography>No weather data available</Typography>
					)}

					<DialogContent
						dividers
						sx={{ mt: 2 }}>
						<Box sx={{ flexGrow: 1 }}>
							<Grid2
								container
								spacing={1}>
								<Grid2 size={6}>
									<Typography
										gutterBottom
										variant="body1"
										sx={{ mt: 2, padding: 0 }}>
										Temperture: {weather?.main.temp} °C
									</Typography>
								</Grid2>
								<Grid2 size={6}>
									<Typography
										gutterBottom
										variant="body1"
										sx={{ mt: 2, padding: 0 }}>
										Feels Like: {weather?.main.feels_like} °C
									</Typography>
								</Grid2>
								<Grid2 size={6}>
									<Typography
										gutterBottom
										variant="body1"
										sx={{ mt: 1, padding: 0 }}>
										Minimum Temp: {weather?.main.temp_min} °C
									</Typography>
								</Grid2>
								<Grid2 size={6}>
									<Typography
										gutterBottom
										variant="body1"
										sx={{ mt: 1, padding: 0 }}>
										Maximum Temp: {weather?.main.temp_max} °C
									</Typography>
								</Grid2>
								<Grid2 size={6}>
									<Typography
										gutterBottom
										variant="body1"
										sx={{ mt: 1, padding: 0 }}>
										Pressure: {weather?.main.pressure} hPa
									</Typography>
								</Grid2>
								<Grid2 size={6}>
									<Typography
										gutterBottom
										variant="body1"
										sx={{ mt: 1, padding: 0 }}>
										Humidity: {weather?.main.humidity} %
									</Typography>
								</Grid2>
								<Grid2 size={6}>
									<Typography
										gutterBottom
										variant="body1"
										sx={{ mt: 1, padding: 0 }}>
										Sea Level Pressure: {weather?.main.sea_level} hPa
									</Typography>
								</Grid2>
								<Grid2 size={6}>
									<Typography
										gutterBottom
										variant="body1"
										sx={{ mt: 1, padding: 0 }}>
										Ground Level Pressure: {weather?.main.grnd_level} hPa
									</Typography>
								</Grid2>
							</Grid2>
						</Box>
					</DialogContent>
				</DialogContent>
			</BootstrapDialog>
		</React.Fragment>
	);
}
