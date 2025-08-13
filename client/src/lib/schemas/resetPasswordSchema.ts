import z from "zod";
import { requiredString } from "../../util/Validator";

export const resetPasswordSchema = z
	.object({
		newPassword: requiredString("newPassword"),
		confirmPassword: requiredString("confirmPassword"),
	})
	.refine((data) => data.newPassword === data.confirmPassword, {
		message: "Password must match",
		path: ["confirmPassword"],
	});

export type ResetPasswordSchema = z.infer<typeof resetPasswordSchema>;
