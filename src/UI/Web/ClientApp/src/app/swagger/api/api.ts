export * from './boards.service';
import { BoardsService } from './boards.service';
export * from './oidcConfiguration.service';
import { OidcConfigurationService } from './oidcConfiguration.service';
export * from './weatherForecast.service';
import { WeatherForecastService } from './weatherForecast.service';
export const APIS = [BoardsService, OidcConfigurationService, WeatherForecastService];
