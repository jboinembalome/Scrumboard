export * from './adherents.service';
import { AdherentsService } from './adherents.service';
export * from './boards.service';
import { BoardsService } from './boards.service';
export * from './cards.service';
import { CardsService } from './cards.service';
export * from './comments.service';
import { CommentsService } from './comments.service';
export * from './oidcConfiguration.service';
import { OidcConfigurationService } from './oidcConfiguration.service';
export * from './teams.service';
import { TeamsService } from './teams.service';
export * from './weatherForecast.service';
import { WeatherForecastService } from './weatherForecast.service';
export const APIS = [AdherentsService, BoardsService, CardsService, CommentsService, OidcConfigurationService, TeamsService, WeatherForecastService];
