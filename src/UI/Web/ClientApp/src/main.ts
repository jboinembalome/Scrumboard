import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

export function getBaseUrl() {
  // Use slice to remove the last slash in the url.
  // It's useful for path which are generated by SwaggerHub.
  return document.getElementsByTagName('base')[0].href.slice(0, -1);
}

const providers = [
  { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] }
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.log(err));

//export { renderModule, renderModuleFactory } from '@angular/platform-server';