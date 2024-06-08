import { NgModule } from '@angular/core';
import { LoginComponent } from './login/login.component';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { IdentityService } from './services/identity.service';
import { LogoutComponent } from './logout/logout.component';
import { MaterialModule } from 'app/shared/material/material.module';


@NgModule({
    imports: [
    MaterialModule,
    HttpClientModule,
    RouterModule.forChild([
        { path: 'login', component: LoginComponent },
        { path: 'logout', component: LogoutComponent }
    ]),
    LoginComponent
],
    providers: [
        IdentityService
    ],
    exports: [LoginComponent]
})
export class AuthModule { }
