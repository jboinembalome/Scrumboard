import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ExtraOptions, PreloadAllModules, RouterModule } from '@angular/router';


import { AppComponent } from './app.component';
import { appRoutes } from './app.routing';
//import { MaterialModule } from './shared/material/material.module';
import { CoreModule } from './core/core.module';
import { LayoutModule } from './layout/layout.module';
import { ApiModule } from './swagger/api.module';
import { AuthModule } from 'src/app/core/auth/auth.module';
import { AuthInterceptor } from 'src/app/core/auth/interceptors/auth.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

const routerConfig: ExtraOptions = {
  preloadingStrategy       : PreloadAllModules,
  scrollPositionRestoration: 'enabled'
};

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    RouterModule.forRoot(appRoutes, routerConfig),

    // Material module with all import
    //MaterialModule,

    // Core module of the application
    CoreModule,

    // Layout module of the application
    LayoutModule,

    // Api module generated by SwaggerHub
    ApiModule,
    
    HttpClientModule,
    FormsModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
