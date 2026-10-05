import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomerService } from '../../core/services/customer.service';
import { Customer } from '../../shared/models/models';

@Component({
  selector: 'sms-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.scss']
})
export class CustomersComponent implements OnInit {
  customers: Customer[] = [];
  searchTerm = '';
  showForm = false;
  editingId: number | null = null;
  form: FormGroup;

  categories = ['Retail', 'Wholesale', 'VIP'];

  constructor(private fb: FormBuilder, private customerService: CustomerService) {
    this.form = this.fb.group({
      code: ['', Validators.required],
      name: ['', Validators.required],
      address: [''],
      mobile: [''],
      email: ['', Validators.email],
      category: ['Retail']
    });
  }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.customerService.getAll(this.searchTerm || undefined).subscribe(list => (this.customers = list));
  }

  newCustomer(): void {
    this.editingId = null;
    this.form.reset({ category: 'Retail' });
    this.form.get('code')?.enable();
    this.showForm = true;
  }

  edit(customer: Customer): void {
    this.editingId = customer.customerId;
    this.form.patchValue(customer);
    this.form.get('code')?.disable(); // code is the natural key, don't let it change after creation
    this.showForm = true;
  }

  save(): void {
    if (this.form.invalid) return;
    const payload = this.form.getRawValue();

    const request$ = this.editingId
      ? this.customerService.update(this.editingId, payload)
      : this.customerService.create(payload);

    request$.subscribe({
      next: () => {
        this.showForm = false;
        this.load();
      },
      error: err => alert(err.error?.message ?? 'Something went wrong while saving the customer.')
    });
  }

  remove(customer: Customer): void {
    if (!confirm(`Delete customer "${customer.name}"?`)) return;
    this.customerService.delete(customer.customerId).subscribe(() => this.load());
  }

  cancel(): void {
    this.showForm = false;
  }
}
