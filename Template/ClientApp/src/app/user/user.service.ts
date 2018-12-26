import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserRegistration, UserLogin } from './user.model';
import { ApiResponse } from '../config/ApiResponse';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  readonly rootUrl = 'http://localhost:4001/';

  constructor(private http: HttpClient) { }
  emailPattern = '^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$';

  registerUser(user: UserRegistration) {
    const body: UserRegistration = {
      Password: user.Password,
      Email: user.Email,
      FirstName: user.FirstName,
      LastName: user.LastName
    };

    return this.http.post(this.rootUrl + 'Register', body);
  }

  login(email, password) {
    const body: UserLogin = {
      Email: email,
      Password: password
    };
    return this.http.post(this.rootUrl + 'authenticate', body);
  }

  test() {
    return this.http.get<ApiResponse>(this.rootUrl + 'test');
  }

  getUserClaims() {
    return this.http.get(this.rootUrl + 'GetUserClaims');
  }
}
