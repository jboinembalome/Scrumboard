import { NgModule, ModuleWithProviders, SkipSelf, Optional } from '@angular/core';
import { Configuration } from './configuration';
import { HttpClient } from '@angular/common/http';


import { AdherentsService } from './api/adherents.service';
import { BoardsService } from './api/boards.service';
import { CardsService } from './api/cards.service';
import { LabelsService } from './api/labels.service';
import { OidcConfigurationService } from './api/oidcConfiguration.service';
import { WeatherForecastService } from './api/weatherForecast.service';

@NgModule({
  imports:      [],
  declarations: [],
  exports:      [],
  providers: [
    AdherentsService,
    BoardsService,
    CardsService,
    LabelsService,
    OidcConfigurationService,
    WeatherForecastService ]
})
export class ApiModule {
    public static forRoot(configurationFactory: () => Configuration): ModuleWithProviders<ApiModule> {
        return {
            ngModule: ApiModule,
            providers: [ { provide: Configuration, useFactory: configurationFactory } ]
        };
    }

    constructor( @Optional() @SkipSelf() parentModule: ApiModule,
                 @Optional() http: HttpClient) {
        if (parentModule) {
            throw new Error('ApiModule is already loaded. Import in your base AppModule only.');
        }
        if (!http) {
            throw new Error('You need to import the HttpClientModule in your AppModule! \n' +
            'See also https://github.com/angular/angular/issues/20575');
        }
    }
}
