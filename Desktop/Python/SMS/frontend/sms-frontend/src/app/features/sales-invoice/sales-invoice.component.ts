import { Component, OnInit } from '@angular/core';
import { SalesService } from '../../core/services/sales.service';
import { CustomerService } from '../../core/services/customer.service';
import { ItemService } from '../../core/services/item.service';
import { Customer, Item, SalesInvoice, SalesLine } from '../../shared/models/models';

type Mode = 'view' | 'add' | 'edit';

@Component({
  selector: 'sms-sales-invoice',
  templateUrl: './sales-invoice.component.html',
  styleUrls: ['./sales-invoice.component.scss']
})
export class SalesInvoiceComponent implements OnInit {
  invoiceType = 'INV';
  invoice: SalesInvoice = this.emptyInvoice();
  mode: Mode = 'view';

  customerList: Customer[] = [];
  itemList: Item[] = [];

  searchTerm = '';
  searchResults: any[] = [];
  showSearch = false;
  showList = false;
  listResults: any[] = [];

  loadingRecord = false;
  errorMessage = '';

  constructor(
    private salesService: SalesService,
    private customerService: CustomerService,
    private itemService: ItemService
  ) {}

  ngOnInit(): void {
    this.customerService.getAll().subscribe(list => (this.customerList = list));
    this.itemService.getAll().subscribe(list => (this.itemList = list));
    this.goLast();
  }

  private emptyInvoice(): SalesInvoice {
    return {
      invoiceType: this.invoiceType,
      invoiceNumber: 0,
      invoiceDate: new Date().toISOString().substring(0, 10),
      customerId: 0,
      invoiceDescription: '',
      lines: []
    };
  }

  get isReadOnly(): boolean {
    return this.mode === 'view';
  }

  get total(): number {
    return this.invoice.lines.reduce((sum, l) => sum + (l.qty * l.price), 0);
  }

  // ---------------- Navigation Bar ----------------

  goFirst(): void {
    this.runNav(this.salesService.first(this.invoiceType));
  }

  goLast(): void {
    this.runNav(this.salesService.last(this.invoiceType));
  }

  goNext(): void {
    if (!this.invoice.invoiceNumber) return;
    this.runNav(this.salesService.next(this.invoiceType, this.invoice.invoiceNumber));
  }

  goPrior(): void {
    if (!this.invoice.invoiceNumber) return;
    this.runNav(this.salesService.prior(this.invoiceType, this.invoice.invoiceNumber));
  }

  private runNav(obs: ReturnType<SalesService['get']>): void {
    this.loadingRecord = true;
    obs.subscribe({
      next: inv => {
        this.invoice = { ...inv, invoiceDate: inv.invoiceDate.substring(0, 10) };
        this.mode = 'view';
        this.loadingRecord = false;
      },
      error: () => {
        this.loadingRecord = false;
        // No record found in that direction (e.g. already at first/last) - keep current view
      }
    });
  }

  openSearch(): void {
    this.showSearch = true;
    this.searchTerm = '';
    this.searchResults = [];
  }

  runSearch(): void {
    this.salesService.search(this.invoiceType, this.searchTerm).subscribe(res => (this.searchResults = res));
  }

  pickSearchResult(row: any): void {
    this.showSearch = false;
    this.salesService.get(this.invoiceType, row.invoiceNumber).subscribe(inv => {
      this.invoice = { ...inv, invoiceDate: inv.invoiceDate.substring(0, 10) };
      this.mode = 'view';
    });
  }

  openList(): void {
    this.showList = true;
    this.salesService.list(this.invoiceType).subscribe(res => (this.listResults = res));
  }

  pickListResult(row: any): void {
    this.showList = false;
    this.salesService.get(this.invoiceType, row.invoiceNumber).subscribe(inv => {
      this.invoice = { ...inv, invoiceDate: inv.invoiceDate.substring(0, 10) };
      this.mode = 'view';
    });
  }

  // ---------------- Updation Bar ----------------

  add(): void {
    this.invoice = this.emptyInvoice();
    this.salesService.getNextInvoiceNumber(this.invoiceType).subscribe(n => (this.invoice.invoiceNumber = n));
    this.mode = 'add';
    this.errorMessage = '';
  }

  edit(): void {
    if (!this.invoice.invoiceNumber) return;
    this.mode = 'edit';
  }

  delete(): void {
    if (!this.invoice.invoiceNumber) return;
    if (!confirm(`Delete invoice #${this.invoice.invoiceNumber}? This cannot be undone.`)) return;

    this.salesService.delete(this.invoiceType, this.invoice.invoiceNumber).subscribe(() => {
      this.goLast();
    });
  }

  save(): void {
    if (!this.invoice.customerId) {
      this.errorMessage = 'Please select a customer.';
      return;
    }
    if (this.invoice.lines.length === 0) {
      this.errorMessage = 'Add at least one line item.';
      return;
    }

    this.errorMessage = '';
    this.salesService.save(this.invoice).subscribe({
      next: saved => {
        this.invoice = { ...saved, invoiceDate: saved.invoiceDate.substring(0, 10) };
        this.mode = 'view';
      },
      error: err => (this.errorMessage = err.error?.message ?? 'Could not save the invoice.')
    });
  }

  cancel(): void {
    if (this.invoice.invoiceNumber && this.mode === 'edit') {
      this.salesService.get(this.invoiceType, this.invoice.invoiceNumber).subscribe(inv => {
        this.invoice = { ...inv, invoiceDate: inv.invoiceDate.substring(0, 10) };
      });
    } else {
      this.goLast();
    }
    this.mode = 'view';
    this.errorMessage = '';
  }

  // ---------------- Line item grid ----------------

  addLine(): void {
    this.invoice.lines.push({ itemId: 0, qty: 1, price: 0 });
  }

  removeLine(index: number): void {
    this.invoice.lines.splice(index, 1);
  }

  onItemChange(line: SalesLine): void {
    const item = this.itemList.find(i => i.itemId === +line.itemId);
    if (item) {
      line.itemDescription = item.description;
      line.itemCode = item.code;
      line.unit = item.unit;
      line.price = item.salesPrice;
    }
  }

  lineAmount(line: SalesLine): number {
    return (line.qty || 0) * (line.price || 0);
  }
}
