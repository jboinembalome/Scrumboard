import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { WeatherForecast } from './fetch-data.model';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  columns: string[] = ['Date', 'Temp. (C)', 'Temp. (F)', 'Summary'];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<WeatherForecast[]>(baseUrl + 'api/weatherforecast').subscribe(result => {
      this.forecasts = result;
      this.forecasts.splice(this.forecasts.length-1, 1);
    }, error => console.error(error));
  }
}