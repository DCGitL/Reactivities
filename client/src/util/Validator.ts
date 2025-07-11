import z from "zod"

export const requiredString = (fieldName: string) => {
    return  z.string({required_error:`${fieldName} is required` })
                .min(4,{message:`${fieldName} is required`})
}