import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { NgClass, AsyncPipe, DatePipe } from '@angular/common';
import { WeatherForecast, WeatherForecastService } from 'app/swagger';

@Component({
    selector: 'app-fetch-data',
    templateUrl: './fetch-data.component.html',
    standalone: true,
    imports: [
      NgClass, 
      AsyncPipe, 
      DatePipe]
})
export class FetchDataComponent implements OnInit {

  columns: string[] = ['Date', 'Temp. (C)', 'Temp. (F)', 'Summary'];
  forecasts: Observable<WeatherForecast[]>;

  constructor(
    private _weatherForecastService : WeatherForecastService) {  
  }

  ngOnInit(): void {
    this.forecasts = this._weatherForecastService.apiWeatherForecastGet();
  }
}