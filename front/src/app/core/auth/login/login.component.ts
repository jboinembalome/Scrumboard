import { Component, OnInit } from "@angular/core";
import { AuthService } from "../services/auth.service";
import { LoginRequest } from "../models/login-request.model";
import { UntypedFormBuilder, UntypedFormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";


@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"],
})
export class LoginComponent implements OnInit {
  loginInForm: UntypedFormGroup;
  
  constructor(
    private _authService: AuthService,
    private _formBuilder: UntypedFormBuilder,
    private _router: Router,) {}

  ngOnInit(): void {
    // Create the form
    this.loginInForm = this._formBuilder.group({
      email     : ['adherent@localhost', [Validators.required, Validators.email]],
      password  : ['Adherent1!', Validators.required]
  });
  }

  email: string = "";
  password: string = "";
  credentials: LoginRequest = {
    email: "",
    password: "",
  };

  login() {
    this._authService
      .login({
        email: this.email,
        password: this.password,
      })
      .subscribe(() => {
        console.log("Login successful");
      });
  }

  signIn(): void
    {
        // Return if the form is invalid
        if ( this.loginInForm.invalid )
        {
            return;
        }

        // Disable the form
        this.loginInForm.disable();

        // Sign in
        this._authService.login(this.loginInForm.value)
            .subscribe(
                () =>
                {
                  this._router.navigate(['/home']);
                },
                (response) =>
                {
                    // Re-enable the form
                    this.loginInForm.enable();

                    // Reset the form
                    this.loginInForm.reset();
                },
            );
    }
}
