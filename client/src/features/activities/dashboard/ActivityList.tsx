import { Box } from "@mui/material";

import ActivityCard from "./ActivityCard";
import { useActivities } from "../../../lib/hooks/useActivities";
import Spinner from "../../../util/Spinner";
import { useInView } from "react-intersection-observer";
import { useEffect } from "react";
import { observer } from "mobx-react-lite";

const ActivityList = observer(function ActivityList() {
	const { activitiesGroup, isPending, hasNextPage, fetchNextPage } =
		useActivities();

	const { ref, inView } = useInView({
		threshold: 0.5,
	});
	useEffect(() => {
		if (inView && hasNextPage) {
			fetchNextPage();
		}
	}, [inView, hasNextPage, fetchNextPage]);

	if (!activitiesGroup || isPending) return <Spinner />; // <Typography>Loading..</Typography>

	return (
		<Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
			{activitiesGroup.pages.map((activities, index) => (
				<Box
					key={index}
					display={"flex"}
					flexDirection={"column"}
					gap={3}
					ref={index === activitiesGroup.pages.length - 1 ? ref : null}>
					{activities.items.map((activity) => (
						<ActivityCard
							key={activity.id}
							activity={activity as Activity}
						/>
					))}
				</Box>
			))}
		</Box>
	);
});
export default ActivityList;
