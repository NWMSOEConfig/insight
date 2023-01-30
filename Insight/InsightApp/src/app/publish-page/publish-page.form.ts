import { FormControl, UntypedFormGroup, Validators } from "@angular/forms";

export class PublishForm {
  /* Public Fields */
  public controls = PublishForm.generateControls();
  public validationGroup = new UntypedFormGroup(this.controls);

  private static generateControls = (): { [key: string]: FormControl } => {
    const controls = {
      commitMessage: new FormControl<string>(""),
      referenceId: new FormControl<number>(0, Validators.min(0)), 
    }
    return controls;
  }

  public canSave = (): boolean => {
    return this.validationGroup.dirty && this.validationGroup.valid;
  }
}
