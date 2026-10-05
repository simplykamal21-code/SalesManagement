import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Item } from '../../shared/models/models';

@Injectable({ providedIn: 'root' })
export class ItemService {
  private base = `${environment.apiUrl}/items`;

  constructor(private http: HttpClient) {}

  getAll(search?: string): Observable<Item[]> {
    const params = search ? { search } : {};
    return this.http.get<Item[]>(this.base, { params });
  }

  getById(id: number): Observable<Item> {
    return this.http.get<Item>(`${this.base}/${id}`);
  }

  create(item: Partial<Item>): Observable<Item> {
    return this.http.post<Item>(this.base, item);
  }

  update(id: number, item: Partial<Item>): Observable<void> {
    return this.http.put<void>(`${this.base}/${id}`, item);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.base}/${id}`);
  }
}
