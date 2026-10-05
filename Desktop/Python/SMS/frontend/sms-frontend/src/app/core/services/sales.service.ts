import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { SalesInvoice } from '../../shared/models/models';

@Injectable({ providedIn: 'root' })
export class SalesService {
  private base = `${environment.apiUrl}/sales`;

  constructor(private http: HttpClient) {}

  getNextInvoiceNumber(invoiceType = 'INV'): Observable<number> {
    return this.http.get<number>(`${this.base}/next-number`, { params: { invoiceType } });
  }

  get(invoiceType: string, invoiceNumber: number): Observable<SalesInvoice> {
    return this.http.get<SalesInvoice>(`${this.base}/${invoiceType}/${invoiceNumber}`);
  }

  first(invoiceType: string): Observable<SalesInvoice> {
    return this.http.get<SalesInvoice>(`${this.base}/${invoiceType}/first`);
  }

  last(invoiceType: string): Observable<SalesInvoice> {
    return this.http.get<SalesInvoice>(`${this.base}/${invoiceType}/last`);
  }

  next(invoiceType: string, currentNumber: number): Observable<SalesInvoice> {
    return this.http.get<SalesInvoice>(`${this.base}/${invoiceType}/${currentNumber}/next`);
  }

  prior(invoiceType: string, currentNumber: number): Observable<SalesInvoice> {
    return this.http.get<SalesInvoice>(`${this.base}/${invoiceType}/${currentNumber}/prior`);
  }

  search(invoiceType: string, term: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.base}/${invoiceType}/search`, { params: { term } });
  }

  list(invoiceType: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.base}/${invoiceType}/list`);
  }

  save(invoice: SalesInvoice): Observable<SalesInvoice> {
    return this.http.post<SalesInvoice>(this.base, invoice);
  }

  delete(invoiceType: string, invoiceNumber: number): Observable<void> {
    return this.http.delete<void>(`${this.base}/${invoiceType}/${invoiceNumber}`);
  }
}
