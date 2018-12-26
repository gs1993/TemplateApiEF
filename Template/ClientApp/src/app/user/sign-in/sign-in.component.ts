import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';
import { ToastrService } from '../../../../node_modules/ngx-toastr';
import { ApiResponse } from '../../config/ApiResponse';
import { Router } from '@angular/router';
import { UserInfo } from '../user.model';
import { Consts } from '../../config/consts';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {
  loginFailed = false;
  errorMessage = '';

  constructor(private userService: UserService, private toastr: ToastrService, private router: Router) { }

  ngOnInit() {
  }

  OnSubmit(email, password) {
    this.userService.login(email, password)
    .toPromise().then((rsp: ApiResponse) => {
      console.log(rsp);
    });
  }

}
