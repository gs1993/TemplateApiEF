import { Component, OnInit } from '@angular/core';
import { UserRegistration } from '../user-registration.model';
import { UserService } from '../user.service';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '../../../../node_modules/@angular/forms';
import { ApiResponse } from '../../config/ApiResponse';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {
  user: UserRegistration;
  emailPattern = '^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$';

  constructor(private userService: UserService, private toastr: ToastrService) { }

  ngOnInit() {
    this.resetForm();
  }

  resetForm(form?: NgForm) {
    if (form != null) {
      form.reset();
    }
    this.user = {
      UserName: '',
      Password: '',
      Email: '',
      FirstName: '',
      LastName: ''
    };
  }

  OnSubmit(form: NgForm) {
    this.userService.registerUser(form.value)
      .subscribe((rsp: ApiResponse) => {
        if (rsp.errorMessage === null) {
          this.resetForm(form);
          this.toastr.success('User registration successful');
        }
      }, error => {
        if (error.error.errorMessage != null) {
          this.toastr.error(error.error.errorMessage);
        }
      });
  }
}
