export default function mergeRefs<T>(...refs: (React.Ref<T> | undefined)[]) {
	return (value: T) => {
		refs.forEach((ref) => {
			if (typeof ref == "function") {
				ref(value);
			} else if (ref && "current" in ref) {
				(ref as React.RefObject<T | null>).current = value;
			}
		});
	};
}
