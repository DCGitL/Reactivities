import { Box, Divider, Typography } from "@mui/material";
import { useParams } from "react-router";
import { useProfile } from "../../lib/hooks/useProfile";
import ProfileCard from "./ProfileCard";

type Props = {
	activeTab: number;
};

export default function ProfileFollowing({ activeTab }: Props) {
	const { id } = useParams();
	const predicate = activeTab == 3 ? "followers" : "followings";
	const { profile, followings, loadingFollowings } = useProfile(id, predicate);

	return (
		<Box>
			<Box>
				<Typography variant="h5">
					{activeTab === 3
						? !loadingFollowings &&
						  followings !== undefined &&
						  followings.length > 0
							? `People following ${profile?.displayName}`
							: `No one following ${profile?.displayName}`
						: !loadingFollowings &&
						  followings !== undefined &&
						  followings.length > 0
						? `People ${profile?.displayName} is following`
						: `${profile?.displayName} is following no one`}
				</Typography>
			</Box>
			<Divider />
			{loadingFollowings ? (
				<Typography>Loading...</Typography>
			) : (
				<Box
					display="flex"
					marginTop={3}
					gap={3}>
					{followings?.map((profile) => (
						<ProfileCard
							key={profile.id}
							profile={profile}
						/>
					))}
				</Box>
			)}
		</Box>
	);
}
