import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountService } from './_services/account.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  showFiller = false;
  title = 'Car Dealership';
  loginModel : any = {
    grant_type: 'password'
  }
  loggedIn = false;

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    
  }
  loginSubmit(){
    var formData: any = new FormData();
    formData.append('username', this.loginModel.username)
    formData.append('password', this.loginModel.password)
    formData.append('grant_type', "password")
    console.log(this.loginModel)
    this.accountService.login(formData).subscribe({
      next: response => {
        console.log(response)
        this.loggedIn = true
      },
      error: error => console.log(error)
    })
  }
}