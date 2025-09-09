import { Visibility, VisibilityOff } from "@mui/icons-material";
import {
	FormControl,
	FormHelperText,
	IconButton,
	InputAdornment,
	InputLabel,
	OutlinedInput,
	type OutlinedInputProps,
} from "@mui/material";
import React from "react";
import {
	useController,
	useFormContext,
	type FieldValues,
	type UseControllerProps,
} from "react-hook-form";

type Props<T extends FieldValues> = {} & UseControllerProps<T> &
	OutlinedInputProps;

export default function InputPasswordIcon<T extends FieldValues>({
	control,
	...props
}: Props<T>) {
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

	const [showPassword, setShowPassword] = React.useState(false);

	const handleClickShowPassword = () => setShowPassword((show) => !show);

	const handleMouseDownPassword = (
		event: React.MouseEvent<HTMLButtonElement>
	) => {
		event.preventDefault();
	};

	const handleMouseUpPassword = (
		event: React.MouseEvent<HTMLButtonElement>
	) => {
		event.preventDefault();
	};
	return (
		<FormControl
			fullWidth
			variant="outlined"
			error={!!fieldState.error}>
			<InputLabel htmlFor={props.id || props.name}>{props.label}</InputLabel>
			<OutlinedInput
				{...props}
				{...field}
				type={showPassword ? "text" : "password"}
				endAdornment={
					<InputAdornment position="end">
						<IconButton
							aria-label={
								showPassword ? "hide the password" : "display the password"
							}
							onClick={handleClickShowPassword}
							onMouseDown={handleMouseDownPassword}
							onMouseUp={handleMouseUpPassword}
							edge="end">
							{showPassword ? (
								<VisibilityOff color="secondary" />
							) : (
								<Visibility color="primary" />
							)}
						</IconButton>
					</InputAdornment>
				}
				error={!!fieldState.error}
			/>
			{fieldState.error && (
				<FormHelperText>{fieldState.error.message}</FormHelperText>
			)}
		</FormControl>
	);
}
