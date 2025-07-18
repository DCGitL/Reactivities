import z from "zod";
import { requiredString } from "../../util/Validator";



export const registerSchema = z.object({
    email: z.string().email(),
    displayName: requiredString('displayName'),
    password: requiredString('password')
 
})

export type RegisterSchema = z.infer<typeof registerSchema>;
