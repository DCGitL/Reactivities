import { Box, Typography } from "@mui/material";
import Countdown, { zeroPad } from "react-countdown";

const renderer = ({
	days,
	hours,
	minutes,
	seconds,
	completed,
}: {
	days: number;
	hours: number;
	minutes: number;
	seconds: number;
	completed: boolean;
}) => {
	return (
		<Box
			sx={{
				paddingTop: "2px",
				paddingBottom: "2px",
				display: "flex",
				justifyContent: "start",
				alignItems: "start",
				backgroundColor: "transparent",
			}}>
			{completed ? (
				<Typography variant="inherit">Activity started </Typography>
			) : (
				<Typography variant="inherit">
					Activity starts in: {days}d : {zeroPad(hours)}h : {zeroPad(minutes)}m
					: {zeroPad(seconds)}s
				</Typography>
			)}
		</Box>
	);
};

type Props = {
	activityDate: Date;
};

export default function CountdownTimer({ activityDate }: Props) {
	return (
		<div>
			<Countdown
				date={activityDate}
				renderer={renderer}
			/>
		</div>
	);
}
