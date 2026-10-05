import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Customer } from '../../shared/models/models';

@Injectable({ providedIn: 'root' })
export class CustomerService {
  private base = `${environment.apiUrl}/customers`;

  constructor(private http: HttpClient) {}

  getAll(search?: string): Observable<Customer[]> {
    const params = search ? { search } : {};
    return this.http.get<Customer[]>(this.base, { params });
  }

  getById(id: number): Observable<Customer> {
    return this.http.get<Customer>(`${this.base}/${id}`);
  }

  create(customer: Partial<Customer>): Observable<Customer> {
    return this.http.post<Customer>(this.base, customer);
  }

  update(id: number, customer: Partial<Customer>): Observable<void> {
    return this.http.put<void>(`${this.base}/${id}`, customer);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.base}/${id}`);
  }
}
