import { NgModule } from '@angular/core';
import { LoginComponent } from './login/login.component';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { IdentityService } from './services/identity.service';
import { LogoutComponent } from './logout/logout.component';
import { MaterialModule } from 'src/app/shared/material/material.module';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [LoginComponent],
  imports: [
    SharedModule,
    MaterialModule,
    HttpClientModule,
    RouterModule.forChild(
      [
        { path: 'login', component: LoginComponent },
        { path: 'logout', component: LogoutComponent }
      ]
    )
  ],
  providers:[
    IdentityService
  ],
  exports: [LoginComponent]
})
export class AuthModule { }
