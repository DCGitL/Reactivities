import { useNavigate, useSearchParams } from "react-router";
import { useAccount } from "../../lib/hooks/useAccount";
import { Typography } from "@mui/material";
import {
	resetPasswordSchema,
	type ResetPasswordSchema,
} from "../../lib/schemas/resetPasswordSchema";
import { LockOpen } from "@mui/icons-material";
import { toast } from "react-toastify";
import AccountFormWrapper from "./AccountFormWrapper";
import { zodResolver } from "@hookform/resolvers/zod";
import TextInput from "../../app/shared/components/TextInput";

export default function ResetPasswordForm() {
	const [params] = useSearchParams();
	const { resetPassword } = useAccount();
	const navigate = useNavigate();
	const email = params.get("email");
	const token = params.get("token");

	if (!email || !token) {
		return <Typography>Invalid reset password code</Typography>;
	}

	const onSubmit = async (data: ResetPasswordSchema) => {
		try {
			console.log("reset formdata", {
				email: email,
				token: token,
				newPassword: data.newPassword,
			});
			await resetPassword.mutateAsync(
				{ email: email, token: token, newPassword: data.newPassword },
				{
					onSuccess: () => {
						toast.success("Password reset successfuly");
						navigate("/login");
					},
				}
			);
		} catch (error) {
			console.log(error);
		}
	};

	return (
		<AccountFormWrapper<ResetPasswordSchema>
			title="Reset your password"
			submitButtonText="Reset password"
			onSubmit={onSubmit}
			resolver={zodResolver(resetPasswordSchema)}
			icon={<LockOpen fontSize="large" />}>
			<TextInput
				label="New password"
				type="password"
				name="newPassword"
			/>
			<TextInput
				label="Confirm password"
				type="password"
				name="confirmPassword"
			/>
		</AccountFormWrapper>
	);
}
