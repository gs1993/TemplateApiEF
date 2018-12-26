import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../user/user.service';
import { Consts } from '../config/consts';
import { ApiResponse } from '../config/ApiResponse';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  userClaims: any;

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit() {
    this.userService.getUserClaims().subscribe((rsp: ApiResponse) => {
      this.userClaims = rsp.data;
    });
  }

  Logout() {
    localStorage.removeItem(Consts.AuthKey);
    this.router.navigate(['/login']);
  }

}
