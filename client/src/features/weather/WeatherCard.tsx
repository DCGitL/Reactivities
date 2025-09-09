import { Box, Card, CardContent, CardHeader, Typography } from "@mui/material";

type Props = {
	weather: WeatherItem[];
};
export default function WeatherCard({ weather }: Props) {
	return (
		<Box
			display="flex"
			height={150}
			gap={2}>
			{weather.map((w: { main: string; description: string; icon: string }) => (
				<Card sx={{ borderRadius: 3, p: 0, maxWidth: 200 }}>
					<CardHeader
						title={w.main}
						avatar={
							<img
								src={w.icon}
								alt={w.description + " icon"}
								style={{ width: 50, height: 50 }}
							/>
						}
					/>
					<CardContent>
						<Typography
							variant="body2"
							color="text.secondary">
							{w.description}
						</Typography>
					</CardContent>
				</Card>
			))}
		</Box>
	);
}
