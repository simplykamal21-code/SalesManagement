import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ItemService } from '../../core/services/item.service';
import { Item } from '../../shared/models/models';

@Component({
  selector: 'sms-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.scss']
})
export class ItemsComponent implements OnInit {
  items: Item[] = [];
  searchTerm = '';
  showForm = false;
  editingId: number | null = null;
  form: FormGroup;
  photoPreview: string | null = null;

  units = ['PCS', 'BOX', 'KG', 'LTR', 'SET'];

  constructor(private fb: FormBuilder, private itemService: ItemService) {
    this.form = this.fb.group({
      code: ['', Validators.required],
      description: ['', Validators.required],
      itemPhoto: [null],
      cost: [0, [Validators.required, Validators.min(0)]],
      salesPrice: [0, [Validators.required, Validators.min(0)]],
      unit: ['PCS'],
      brand: [''],
      supplier: ['']
    });
  }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.itemService.getAll(this.searchTerm || undefined).subscribe(list => (this.items = list));
  }

  newItem(): void {
    this.editingId = null;
    this.form.reset({ cost: 0, salesPrice: 0, unit: 'PCS' });
    this.form.get('code')?.enable();
    this.photoPreview = null;
    this.showForm = true;
  }

  edit(item: Item): void {
    this.editingId = item.itemId;
    this.form.patchValue(item);
    this.form.get('code')?.disable();
    this.photoPreview = item.itemPhoto ?? null;
    this.showForm = true;
  }

  onPhotoSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) return;

    const file = input.files[0];
    const reader = new FileReader();
    reader.onload = () => {
      const base64 = reader.result as string;
      this.photoPreview = base64;
      this.form.patchValue({ itemPhoto: base64 });
    };
    reader.readAsDataURL(file);
  }

  save(): void {
    if (this.form.invalid) return;
    const payload = this.form.getRawValue();

    const request$ = this.editingId
      ? this.itemService.update(this.editingId, payload)
      : this.itemService.create(payload);

    request$.subscribe({
      next: () => {
        this.showForm = false;
        this.load();
      },
      error: err => alert(err.error?.message ?? 'Something went wrong while saving the item.')
    });
  }

  remove(item: Item): void {
    if (!confirm(`Delete item "${item.description}"?`)) return;
    this.itemService.delete(item.itemId).subscribe(() => this.load());
  }

  cancel(): void {
    this.showForm = false;
  }
}
