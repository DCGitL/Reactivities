import { Person } from "@mui/icons-material";
import {
	Box,
	Card,
	CardContent,
	CardMedia,
	Chip,
	Divider,
	Typography,
} from "@mui/material";
import { Link } from "react-router";

interface Props {
	profile: Profile;
}

export default function ProfileCard({ profile }: Props) {
	const following = false;
	return (
		<Link
			to={`/profiles/${profile.id}`}
			style={{ textDecoration: "none" }}>
			<Card
				sx={{ borderRadius: 3, p: 3, maxWidth: 300, textDecoration: "none" }}
				elevation={4}>
				<CardMedia
					component={"img"}
					src={profile?.imageUrl || "/images/user.png"}
					sx={{ width: "100%", zIndex: 50 }}
					alt={profile.displayName + " image"}
				/>
				<CardContent>
					<Box
						display="flex"
						flexDirection={"column"}
						gap={1}>
						<Typography variant="h5">{profile.displayName}</Typography>
						{profile.bio && (
							<Typography
								variant="body2"
								sx={{
									textOverflow: "ellipsis",
									overflow: "hidden",
									whiteSpace: "nowrap",
								}}>
								{profile.bio}
							</Typography>
						)}
						{following && (
							<Chip
								size="small"
								label="following"
								color="secondary"
								variant="outlined"
							/>
						)}
					</Box>
				</CardContent>
				<Divider sx={{ mb: 2 }} />
				<Box
					sx={{
						display: "flex",
						alignItems: "center",
						justifyContent: "start",
					}}>
					<Person />
					<Typography sx={{ ml: 2 }}>20 Following</Typography>
				</Box>
			</Card>
		</Link>
	);
}
