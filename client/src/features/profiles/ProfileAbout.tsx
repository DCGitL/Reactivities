import { useParams } from "react-router";

import { Box, Button, Divider, Typography } from "@mui/material";
import { useState } from "react";
import ProfileEditForm from "./ProfileEditForm";
import { useQueryClient } from "@tanstack/react-query";

export default function ProfileAbout() {
	const { id } = useParams();
	//const { isCurrentUser } = useProfile(id);
	const queryClient = useQueryClient();
	const profile = queryClient.getQueryData<Profile>(["profiles", id]);
	const isCurrentUser = id === queryClient.getQueryData<User>(["user"])?.id;
	const [editMode, setEditMode] = useState(false);

	return (
		<Box>
			<Box
				display="flex"
				justifyContent={"space-between"}>
				<Typography variant="h5">About {profile?.displayName}</Typography>
				{isCurrentUser && (
					<Button onClick={() => setEditMode(!editMode)}>
						{editMode ? "Edit Profile" : "Cancel"}
					</Button>
				)}
			</Box>
			<Divider sx={{ my: 2 }} />
			<Box sx={{ overflow: "auto", height: 350 }}>
				{editMode ? (
					<ProfileEditForm setEditMode={setEditMode} />
				) : (
					<Typography
						variant="body1"
						sx={{ whiteSpace: "pre-wrap" }}>
						{profile?.bio || "No bio available as yet"}
					</Typography>
				)}
			</Box>
		</Box>
	);
}
