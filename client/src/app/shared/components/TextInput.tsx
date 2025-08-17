import { TextField, type TextFieldProps } from "@mui/material";
import { forwardRef, type JSX } from "react";

import {
	useController,
	useFormContext,
	type FieldValues,
	type UseControllerProps,
} from "react-hook-form";
import mergeRefs from "../../../util/MergeRef";

type Props<T extends FieldValues> = {} & UseControllerProps<T> & TextFieldProps;
const TextInput = forwardRef(function TextInputInner<T extends FieldValues>(
	{ control, onChange, ...props }: Props<T>,
	ref?: React.Ref<HTMLInputElement>
) {
	const formContext = useFormContext<T>();
	const effectiveControl = control || formContext?.control;

	if (!effectiveControl) {
		throw new Error(
			"Text input must be used within a form provider or passd as props"
		);
	}

	const { field, fieldState } = useController({
		...props,
		control: effectiveControl,
	});
	const handleChange: TextFieldProps["onChange"] = (event) => {
		field.onChange(event); //update RHF => (react hook form) state
		onChange?.(event); //run caller's handler if provider
	};
	return (
		<TextField
			{...props}
			{...field}
			inputRef={mergeRefs(field.ref, ref)}
			onChange={handleChange}
			value={field.value ?? ""}
			fullWidth
			variant="outlined"
			error={!!fieldState.error}
			helperText={fieldState.error?.message}
		/>
	);
}) as <T extends FieldValues>(
	p: Props<T> & { ref?: React.Ref<HTMLInputElement> }
) => JSX.Element;
export default TextInput;
