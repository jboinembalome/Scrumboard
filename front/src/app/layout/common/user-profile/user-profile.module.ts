import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LoginMenuComponent } from './login-menu/login-menu.component';
import { UserProfileComponent } from './user-profile.component';
import { AuthModule } from 'app/core/auth/auth.module';
import { MaterialModule } from 'app/shared/material/material.module';
import { ComponentModule  } from 'app/shared/components/component.module';
import { SharedModule } from 'app/shared/shared.module';


@NgModule({
    imports: [
        RouterModule,
        AuthModule,
        ComponentModule,
        SharedModule,
        MaterialModule,
        UserProfileComponent,
        LoginMenuComponent
    ],
    exports: [
        UserProfileComponent,
        LoginMenuComponent
    ]
})
export class UserProfileModule
{
}
