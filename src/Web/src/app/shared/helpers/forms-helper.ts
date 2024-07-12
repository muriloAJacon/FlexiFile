import { AbstractControl, FormArray, FormGroup, ValidationErrors } from "@angular/forms";

export class FormsHelper {
	public static markFormAsDirty(form: FormGroup | FormArray): void {
		if (form instanceof FormGroup) {
			for (const key in form.controls) {
				form.controls[key].markAsDirty();
				form.controls[key].markAsTouched();
			}
		} else {
			for (const key in form.controls) {
				form.controls[key].markAsDirty();
				form.controls[key].markAsTouched();
			}
		}
	}

	public static updateValueAndValidity(form: FormGroup): void {
		form.updateValueAndValidity();

		for (const key in form.controls) {
			form.controls[key].updateValueAndValidity();
		}
	}

	public static removeAllFormGroupControls(form: FormGroup) {
		for (const control in form.controls) {
			form.removeControl(control);
		}
	}

	public static strongPasswordValidator(control: AbstractControl): ValidationErrors | null {
		const value = control.value as string | null;

		if (value === null) {
			return null;
		}

		let errors: ValidationErrors = {};

		if (!/[A-Z]/.test(value)) {
			errors["noUpperCase"] = true;
		}

		if (!/[0-9]/.test(value)) {
			errors["noNumber"] = true;
		}

		return errors;
	}

	public static sizeUnits = [
		{ value: 1, description: "B" },
		{ value: 1000, description: "KB" },
		{ value: 1000000, description: "MB" },
		{ value: 1000000000, description: "GB" },
	];

	public static getSizeUnit(size: number): { value: number, description: string } {
		const sizeUnitsCopy = [...this.sizeUnits];
		sizeUnitsCopy.sort((a, b) => b.value - a.value);
		for (let i = 0; i < sizeUnitsCopy.length; i++) {
			if (size % sizeUnitsCopy[i].value !== size) {
				return sizeUnitsCopy[i];
			}
		}

		return sizeUnitsCopy[sizeUnitsCopy.length - 1];
	}
}