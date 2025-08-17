import { Fragment, useRef, useState } from "react";
import { loginSchema, type LoginSchema } from "../../lib/schemas/loginSchema";
import AccountFormWrapper from "./AccountFormWrapper";
import { useAccount } from "../../lib/hooks/useAccount";
import { Link, useLocation, useNavigate } from "react-router";
import { LockOpen } from "@mui/icons-material";
import TextInput from "../../app/shared/components/TextInput";
import {
	Box,
	Button,
	Paper,
	Typography,
	type SxProps,
	type Theme,
} from "@mui/material";
import { zodResolver } from "@hookform/resolvers/zod";
import { toast } from "react-toastify";

export default function LoginformWithWrapper() {
	const [notVerified, setNotVerified] = useState<boolean>(false);
	const { loginUser, resendConfirmationEmail } = useAccount();
	const navigate = useNavigate();
	const location = useLocation();

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
	//const [email, setEmail] = useState<string>("");

	const emailRef = useRef<HTMLInputElement>(null);

	const handleResendEmail = async () => {
		const email = emailRef.current?.value;
		if (!email) {
			toast.error("Email address cannot be empty");
			return;
		}
		console.log(email);
		try {
			await resendConfirmationEmail.mutateAsync({ email });
			setNotVerified(false);
		} catch (error) {
			console.log(error);
			toast.error("Problem sending email - please check your email address");
		}
	};

	const apformLoginTheme: SxProps<Theme> = {
		display: "flex",
		flexDirection: "column",
		p: 3,
		gap: 3,
		maxWidth: "md",
		mx: "auto",
		borderTopLeftRadius: 10,
		borderTopRightRadius: 10,
	};

	return (
		<Fragment>
			<Paper
				sx={{
					maxWidth: "md",
					mx: "auto",
					borderRadius: 10,
					position: "relative",
					height: 370,
				}}>
				<AccountFormWrapper<LoginSchema>
					title="Sign In"
					submitButtonText="Login"
					icon={<LockOpen fontSize="large" />}
					onSubmit={onSubmit}
					resolver={zodResolver(loginSchema)}
					apformTheme={apformLoginTheme}>
					<TextInput
						name="email"
						label="Email"
						// onChange={(event) => {
						// 	setEmail(event.target.value);
						// 	console.log(event.target.value);
						// }}
						ref={emailRef}
					/>
					<TextInput
						name="password"
						type="password"
						label="Password"
					/>
				</AccountFormWrapper>

				<Paper
					sx={{
						position: "absolute",
						marginTop: 0,
						bottom: 0,
						display: "flex",
						justifyContent: "center",
						paddingBottom: 4,
						width: "100%",
						borderBottomRightRadius: 10,
						borderBottomLeftRadius: 10,
						zIndex: 5,
					}}>
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
							maxWidth={"md"}
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
			</Paper>
		</Fragment>
	);
}
