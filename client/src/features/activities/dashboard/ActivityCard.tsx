import { AccessTime, Place } from "@mui/icons-material";
import {
	Avatar,
	Box,
	Button,
	Card,
	CardContent,
	CardHeader,
	Chip,
	Divider,
	Typography,
} from "@mui/material";
import { Link } from "react-router";
import { formatDate } from "../../../util/DateFormater";
import AvatarPopover from "../../../app/shared/components/AvatarPopover";
type Props = {
	activity: Activity;
};

export default function ActivityCard({ activity }: Props) {
	const label = activity.isHost ? "You are hosting" : "You are going";
	const color = activity.isHost
		? "secondary"
		: activity.isGoing
		? "warning"
		: "default";

	return (
		<Card
			elevation={3}
			sx={{ borderRadius: 3 }}>
			<Box
				display="flex"
				alignItems="center"
				justifyContent={"space-between"}>
				<CardHeader
					avatar={
						<Avatar
							src={activity.hostImageUrl}
							sx={{ heith: 40, width: 40 }}
							alt={"image of host"}
						/>
					}
					title={activity.title}
					slotProps={{
						fontWeitht: "bold",
						fontSize: 20,
					}}
					subheader={
						<>
							Hosteb by{" "}
							<Link to={`/profiles/${activity.hostId}`}>
								{activity.hostDisplayName}
							</Link>
						</>
					}
				/>
				<Box
					display={"flex"}
					flexDirection="column"
					gap={2}
					mr={2}>
					{(activity.isHost || activity.isGoing) && (
						<Chip
							variant="outlined"
							label={label}
							color={color}
							sx={{ borderRadius: 2 }}
						/>
					)}
					{activity.isCancelled && (
						<Chip
							label="Cancelled"
							color="error"
							sx={{ borderRadius: 2 }}
						/>
					)}
				</Box>
			</Box>
			<Divider sx={{ mb: 3 }} />

			<CardContent sx={{ padding: 0 }}>
				<Box
					display={"flex"}
					alignItems={"center"}
					mb={2}
					paddingX={2}>
					<Box
						display={"flex"}
						flexGrow={0}
						alignItems={"center"}>
						<AccessTime sx={{ mr: 1 }} />
						<Typography
							variant="body2"
							noWrap>
							{formatDate(activity.date)}
						</Typography>
					</Box>
					<Place sx={{ ml: 3, mr: 1 }} />
					<Typography variant="body2">{activity.venue}</Typography>
				</Box>
				<Divider />
				<Box
					display={"flex"}
					gap={2}
					sx={{ backgroundColor: "gray.200", py: 3, pl: 3 }}>
					{activity.attendees.map((att) => (
						<AvatarPopover
							key={att.id}
							profile={att}
						/>
					))}
				</Box>
			</CardContent>
			<CardContent sx={{ pb: 2 }}>
				<Typography variant="body2">{activity.description}</Typography>
				<Box
					sx={{ display: "flex", justifyContent: "flex-end", borderRadius: 3 }}>
					<Button
						size="medium"
						component={Link}
						to={`/activities/${activity.id}`}
						variant="contained"
						sx={{ borderRadius: 3 }}>
						View
					</Button>
				</Box>
			</CardContent>
		</Card>
	);
}
