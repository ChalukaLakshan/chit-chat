import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatSnackBar } from "@angular/material";
import { Router } from "@angular/router";
import { UserService } from "src/app/services/user.service";
import { User } from "../../models/user";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.css"],
})
export class RegisterComponent implements OnInit {
  public isLoading: boolean = true;
  public formModel: FormGroup;

  constructor(
    private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.formModel = this.fb.group({
      username: ["", Validators.required],
      credentials: this.fb.group(
        {
          password: [
            "",
            [
              Validators.required,
              Validators.pattern(
                "^((?=.*?[A-z])(?=.*?[0-9])|(?=.*?[A-Z])).{8,}$"
              ),
            ],
          ],
          confirmPassword: ["", Validators.required],
        },
        { validator: this.passwordConfirming }
      ),
    });
    this.isLoading = false;
  }

  private passwordConfirming(fb: FormGroup): void {
    let passwordController = fb.get("confirmPassword");
    if (
      passwordController.errors == null ||
      "notUnique" in passwordController.errors
    ) {
      if (fb.get("password").value !== passwordController.value) {
        passwordController.setErrors({
          notUnique: true,
        });
      } else {
        passwordController.setErrors(null);
      }
    }
  }

  public onSubmit(): void {
    const user = new User();
    user.username = this.formModel.value.username;
    user.password = this.formModel.value.credentials.password;

    this.userService.register(user).subscribe(
      (response) => {
        this.formModel.reset();
        this.snackBar.open(
          "Well done! You successfully registered with the Chit-Chat",
          "Dance",
          { duration: 5 * 1000 }
        );

        this.router.navigate([""]);
        this.isLoading = false;
      },
      (error) => {
        if (error && error.error && error.error.message) {
          this.snackBar.open("ERROR", error.error.message);
        }
        this.isLoading = false;
      }
    );
  }
}
