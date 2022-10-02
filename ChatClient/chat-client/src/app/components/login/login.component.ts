import { Component, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";
import { MatSnackBar } from "@angular/material";
import { Router } from "@angular/router";
import { UserService } from "src/app/services/user.service";
import { User } from "../../models/user";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"],
})
export class LoginComponent implements OnInit {
  isLoading: boolean = true;
  username: string;
  password: string;
  userModel: User = new User();

  constructor(
    private snackBar: MatSnackBar,
    private router: Router,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.isLoading = false;
    window.sessionStorage.removeItem("token");
    window.sessionStorage.removeItem("username");
  }

  public onSubmit(form: NgForm): void {
    this.isLoading = true;

    const user: User = new User();
    user.username = form.value.username;
    user.password = form.value.password;

    this.userService.login(user).subscribe(
      (response) => {

        if (response && response.token) {
          window.sessionStorage.setItem("token", response.token);
          window.sessionStorage.setItem("username", response.username);    
        }

        this.snackBar.open("Well done! You logged to the Chit-Chat", "Dance", {
          duration: 4 * 1000,
        });

        this.router.navigateByUrl("inbox");
        this.isLoading = false;
      },
      (error) => {
        if (error && error.error && error.error.message) {
          this.snackBar.open('ERROR', error.error.message);
        }
        this.isLoading = false;
      }
    );   
  }
}
