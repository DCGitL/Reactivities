import {
	Box,
	Card,
	CardContent,
	CardMedia,
	Grid2,
	Tab,
	Tabs,
	Typography,
} from "@mui/material";
import { useProfile } from "../../lib/hooks/useProfile";
import { Link, useParams } from "react-router";
import { useEffect, useState, type SyntheticEvent } from "react";
import { formatDate } from "../../util/DateFormater";

export default function ProfileActivities() {
	const { id } = useParams();
	const { userActivities, userActivitiesLoading, setFilter } = useProfile(id);

	const [activeTab, setActiveTab] = useState(0);

	useEffect(() => {
		setFilter("future");
	}, [setFilter]);

	const handleChange = (_: SyntheticEvent, newValue: number) => {
		setActiveTab(newValue);
		const filter = tabContents[newValue].eventKey;
		setFilter(filter);
	};

	const tabContents = [
		{ eventKey: "future", eventValue: "Future Event" },
		{ eventKey: "past", eventValue: "Past Event" },
		{ eventKey: "hosting", eventValue: "Event hosting" },
	];

	if (userActivitiesLoading) return <Typography>Loading....</Typography>;

	return (
		<Box>
			<Grid2
				container
				sx={{ borderBottom: 1, borderColor: "divider" }}>
				<Grid2 size={12}>
					<Tabs
						value={activeTab}
						onChange={handleChange}>
						{tabContents.map((content, index) => (
							<Tab
								key={index}
								label={content.eventValue}
							/>
						))}
					</Tabs>
				</Grid2>
			</Grid2>
			<Grid2
				container
				size={12}
				spacing={2}
				sx={{ marginTop: 2, height: 400, overflow: "auto" }}>
				{userActivities && userActivities.length > 0 ? (
					userActivities?.map((userActivity) => (
						<Grid2
							key={userActivity.id}
							size={{ xs: 2 }}>
							<Link
								to={`/activities/${userActivity.id}`}
								style={{ textDecoration: "none" }}>
								<Card
									sx={{
										borderRadius: 3,
										p: 2,
										maxWidth: 300,
										textDecoration: "none",
									}}>
									<CardMedia
										component={"img"}
										src={`/images/categoryImages/${userActivity.category}.jpg`}
										sx={{ width: "100%", height: 140, borderRadius: 3 }}
										alt={userActivity.title + " image"}
									/>
									<CardContent sx={{ padding: 0 }}>
										<Typography
											gutterBottom
											variant="h6">
											{userActivity.title}
										</Typography>
										<Typography
											variant="body2"
											sx={{ color: "text.secondary" }}>
											Event date {formatDate(userActivity.date)}
										</Typography>
									</CardContent>
								</Card>
							</Link>
						</Grid2>
					))
				) : (
					<Box>
						<Typography variant="h6">No Event available to display</Typography>
					</Box>
				)}
			</Grid2>
		</Box>
	);
}
