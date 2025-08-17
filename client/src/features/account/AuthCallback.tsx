import { useNavigate, useSearchParams } from "react-router";
import { useAccount } from "../../lib/hooks/useAccount";
import { useEffect, useRef, useState } from "react";
import { Box, CircularProgress, Paper, Typography } from "@mui/material";
import { GitHub } from "@mui/icons-material";

export default function AuthCallback() {
	const [params] = useSearchParams();
	const navigate = useNavigate();
	const code = params.get("code");
	const { fetchGithubToken } = useAccount();
	const [loading, setLoading] = useState<boolean>(true);

	const fetch = useRef<boolean>(false);

	useEffect(() => {
		if (!code || fetch.current) return;
		fetch.current = true;

		fetchGithubToken
			.mutateAsync(code)
			.then(() => {
				return navigate("/activities");
			})
			.catch((error) => {
				console.log(error);
				setLoading(false);
			});
	}, [code, fetchGithubToken, navigate]);
	if (!code) return <Typography>Problem authenticating with GitHub</Typography>;
	return (
		<Paper
			sx={{
				height: 400,
				display: "flex",
				flexDirection: "column",
				alignItems: "center",
				justifyContent: "center",
				p: 3,
				gap: 3,
				maxWidth: "md",
				mx: "auto",
				borderRadius: 3,
			}}>
			<Box
				display="flex"
				alignItems="center"
				justifyContent="center"
				gap={3}>
				<GitHub fontSize="large" />
				<Typography variant="h4">Logging in with GitHub</Typography>
			</Box>
			{loading ? (
				<CircularProgress />
			) : (
				<Typography>Problem sigining in with github</Typography>
			)}
		</Paper>
	);
}
