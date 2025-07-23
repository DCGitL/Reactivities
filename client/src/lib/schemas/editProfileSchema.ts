import z from "zod";
import { requiredString } from "../../util/Validator";

export const editProfileSchema = z.object({
	displayName: requiredString("displayName"),
	bio: z.string().optional(),
});
export type EditProfileSchema = z.infer<typeof editProfileSchema>;
