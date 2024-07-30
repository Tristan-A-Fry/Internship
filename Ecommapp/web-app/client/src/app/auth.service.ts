// src/app/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'http://localhost:5078/api/auth'; 

  constructor(private http: HttpClient, private router: Router) {}

  login(credentials: { username: string; password: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials).pipe(
      tap((response) => {
        console.log("Response from server: ", response);
        localStorage.setItem('token', response.token);
        localStorage.setItem('username', credentials.username);
        localStorage.setItem('roles', JSON.stringify(response.roles));
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    localStorage.removeItem('roles');
    this.router.navigate(['/login']);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }

  isAuthorized() : boolean{
    const roles = localStorage.getItem('roles');
    console.log(typeof(roles));
    if (roles) {
      const rolesArray: string[] = JSON.parse(roles);
      return rolesArray.includes('Admin');
    }
    return false;
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }
  getUsername(): string | null {
    return localStorage.getItem('username');
  }
  getRole(): string[] {
    const roles =  localStorage.getItem('roles');
    console.log("roles: ", roles)
    return roles ? JSON.parse(roles) : [];
  }
}

