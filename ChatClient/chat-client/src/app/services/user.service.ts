import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { User } from "../models/user";

@Injectable({
  providedIn: "root",
})
export class UserService {
  private baseUrl = environment.baseApi;

  constructor(private http: HttpClient) {}

  public login(user: User): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}api/users/login`, user);
  }

  public register(user: User): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}api/users/register`, user);
  }

  public getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${environment.baseApi}api/users`, {
      headers: this.getHttpHeaders(),
    });
  }

  public getUserByName(username: string): Observable<User> {
    return this.http.get<User>(`${environment.baseApi}api/users/name/${username}`, {
      headers: this.getHttpHeaders(),
    });
  }

  private getHttpHeaders(): HttpHeaders {
    return new HttpHeaders().append(
      "Authorization",
      `Bearer ${sessionStorage.getItem("token")}`
    );
  }
}
