import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserRegistration } from './user-registration.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  readonly rootUrl = 'http://localhost:4001/';

  constructor(private http: HttpClient) { }

  registerUser(user: UserRegistration) {
    const body: UserRegistration = {
      UserName: user.UserName,
      Password: user.Password,
      Email: user.Email,
      FirstName: user.FirstName,
      LastName: user.LastName
    };

    return this.http.post(this.rootUrl + 'Register', body);
  }
}
