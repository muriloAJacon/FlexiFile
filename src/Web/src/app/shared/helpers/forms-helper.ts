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
}