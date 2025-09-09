import { useForm } from "react-hook-form";
import { useAccount } from "../../lib/hooks/useAccount";
import { loginSchema, type LoginSchema } from "../../lib/schemas/loginSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, Button, Paper, Typography } from "@mui/material";
import { GitHub, LockOpen } from "@mui/icons-material";
import TextInput from "../../app/shared/components/TextInput";
import { Link, useLocation, useNavigate } from "react-router";
import { useState } from "react";
import { toast } from "react-toastify";
import Config from "../../util/Config";

import InputPasswordIcon from "../../app/shared/components/InputPasswordIcon";

export default function LoginForm() {
	const [notVerified, setNotVerified] = useState<boolean>(false);
	const { loginUser, resendConfirmationEmail } = useAccount();
	const navigate = useNavigate();
	const location = useLocation();

	const {
		control,
		handleSubmit,
		watch,
		formState: { isValid, isSubmitting },
	} = useForm<LoginSchema>({
		mode: "onTouched",
		resolver: zodResolver(loginSchema),
		defaultValues: {
			email: "",
			password: "",
		},
	});
	const email = watch("email");

	const handleResendEmail = async () => {
		try {
			await resendConfirmationEmail.mutateAsync({ email });
			setNotVerified(false);
		} catch (error) {
			console.log(error);
			toast.error("Problem sending email - please check your email address");
		}
	};

	const onSubmit = async (data: LoginSchema) => {
		await loginUser.mutateAsync(data, {
			onSuccess: () => {
				navigate(location.state?.from || "/activities");
			},
			onError: (error) => {
				if (error.message === "NotAllowed") {
					setNotVerified(true);
				}
			},
		});
	};
	const loginWithGithub = () => {
		const clientId = Config.clientID;
		const redirectUrl = Config.redirectUrl;
		window.location.href =
			Config.githubOAuthUrl +
			`?client_id=${clientId}&redirect_uri=${redirectUrl}&scope=read:user user:email`;
	};

	return (
		<Paper
			component={"form"}
			onSubmit={handleSubmit(onSubmit)}
			sx={{
				display: "flex",
				flexDirection: "column",
				p: 3,
				gap: 3,
				maxWidth: "md",
				mx: "auto",
				borderRadius: 3,
			}}>
			<Box
				display="flex"
				alignItems={"center"}
				justifyContent={"center"}
				gap={3}
				color="secondary.main">
				<LockOpen fontSize="large" />
				<Typography variant="h4">Sign in</Typography>
			</Box>
			<TextInput
				label="Email"
				type="email"
				id="email"
				control={control}
				name="email"
			/>
			<InputPasswordIcon
				label="Password"
				id="password"
				control={control}
				name="password"
			/>
			<Button
				type="submit"
				disabled={!isValid || isSubmitting}
				variant="contained"
				size="large">
				Login
			</Button>
			<Button
				onClick={loginWithGithub}
				sx={{ backgroundColor: "black" }}
				startIcon={<GitHub />}
				type="button"
				variant="contained"
				size="large">
				Login with Github
			</Button>
			{notVerified ? (
				<Box
					display="flex"
					flexDirection={"column"}
					justifyContent={"center"}>
					<Typography
						textAlign="center"
						color="error">
						Your email has not yet varified. You can click the resend button
					</Typography>
					<Button
						disabled={resendConfirmationEmail.isPending}
						onClick={handleResendEmail}>
						Re-send email
					</Button>
				</Box>
			) : (
				<Box
					display="flex"
					alignItems={"center"}
					justifyContent={"center"}
					gap={3}>
					<Typography>
						Forgot password! Click <Link to="/forgot-password">here</Link>
					</Typography>

					<Typography sx={{ textAlign: "center" }}>
						Don't have an account?
						<Typography
							sx={{ ml: 2 }}
							component={Link}
							to="/register"
							color="primary">
							Sign up
						</Typography>
					</Typography>
				</Box>
			)}
		</Paper>
	);
}
