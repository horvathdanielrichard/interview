import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CreateMissionRequest, Mission } from '../../core/models/mission.model';

export interface MissionFormDialogData {
  mission: Mission | null;
}

@Component({
  selector: 'app-mission-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  templateUrl: './mission-form-dialog.html',
  styleUrl: './mission-form-dialog.scss',
})
export class MissionFormDialog {
  private readonly fb = new FormBuilder();
  readonly isEdit: boolean;

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    description: ['', [Validators.required, Validators.maxLength(2000)]],
    requiredStrength: [10, [Validators.required, Validators.min(0)]],
    requiredSpeed: [10, [Validators.required, Validators.min(0)]],
    requiredIntelligence: [10, [Validators.required, Validators.min(0)]],
    requiredDurability: [10, [Validators.required, Validators.min(0)]],
    requiredEnergy: [10, [Validators.required, Validators.min(0)]],
    durationMinutes: [5, [Validators.required, Validators.min(1)]],
  });

  constructor(
    private readonly dialogRef: MatDialogRef<MissionFormDialog, CreateMissionRequest>,
    @Inject(MAT_DIALOG_DATA) data: MissionFormDialogData,
  ) {
    this.isEdit = data.mission !== null;

    if (data.mission) {
      const [hours, minutes] = data.mission.duration.split(':').map(Number);
      this.form.patchValue({
        name: data.mission.name,
        description: data.mission.description,
        requiredStrength: data.mission.requiredStrength,
        requiredSpeed: data.mission.requiredSpeed,
        requiredIntelligence: data.mission.requiredIntelligence,
        requiredDurability: data.mission.requiredDurability,
        requiredEnergy: data.mission.requiredEnergy,
        durationMinutes: hours * 60 + minutes,
      });
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();
    const totalMinutes = value.durationMinutes;
    const hours = Math.floor(totalMinutes / 60);
    const minutes = totalMinutes % 60;
    const duration = `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}:00`;

    this.dialogRef.close({
      name: value.name,
      description: value.description,
      requiredStrength: value.requiredStrength,
      requiredSpeed: value.requiredSpeed,
      requiredIntelligence: value.requiredIntelligence,
      requiredDurability: value.requiredDurability,
      requiredEnergy: value.requiredEnergy,
      duration,
    });
  }
}
