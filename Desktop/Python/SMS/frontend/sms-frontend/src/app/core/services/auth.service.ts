import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';

interface LoginResponse {
  token: string;
  name: string;
  username: string;
  expiresAt: string;
}

const TOKEN_KEY = 'sms_token';
const USER_KEY = 'sms_user';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${environment.apiUrl}/auth/login`, { username, password })
      .pipe(
        tap(res => {
          localStorage.setItem(TOKEN_KEY, res.token);
          localStorage.setItem(USER_KEY, JSON.stringify({ name: res.name, username: res.username }));
        })
      );
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  get currentUserName(): string {
    const raw = localStorage.getItem(USER_KEY);
    return raw ? JSON.parse(raw).name : '';
  }
}
