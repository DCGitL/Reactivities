import { useForm } from "react-hook-form";
import {
	editProfileSchema,
	type EditProfileSchema,
} from "../../lib/schemas/editProfileSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import TextInput from "../../app/shared/components/TextInput";
import { Box, Button } from "@mui/material";
import { useParams } from "react-router";
import { useProfile } from "../../lib/hooks/useProfile";
import { useEffect } from "react";

type Props = {
	setEditMode: (editMode: boolean) => void;
};

export default function ProfileEditForm({ setEditMode }: Props) {
	const { id } = useParams();
	const { updateEditProfile, profile } = useProfile(id);

	const {
		control,
		handleSubmit,
		reset,
		formState: { isDirty, isValid },
	} = useForm<EditProfileSchema>({
		mode: "onTouched",
		resolver: zodResolver(editProfileSchema),
	});
	const onSubmit = async (data: EditProfileSchema) => {
		// Handle form submission logic here
		await updateEditProfile.mutateAsync(data, {
			onSuccess: () => setEditMode(false),
		});
	};
	// Reset form with current profile data when profile changes
	useEffect(() => {
		reset({
			displayName: profile?.displayName,
			bio: profile?.bio,
		});
	}, [profile, reset]);

	return (
		<Box
			component={"form"}
			onSubmit={handleSubmit(onSubmit)}
			sx={{
				display: "flex",
				flexDirection: "column",
				aligContent: "center",
				mt: 3,
				gap: 3,
				maxWidth: "md",
				mx: "auto",
				borderRadius: 3,
			}}>
			<TextInput
				label="Display Name"
				control={control}
				name="displayName"
			/>
			<TextInput
				label="Bio"
				control={control}
				name="bio"
				multiline
				rows={3}
			/>
			<Button
				type="submit"
				variant="contained"
				disabled={!isDirty || !isValid || updateEditProfile.isPending}>
				Update Profile
			</Button>
		</Box>
	);
}
