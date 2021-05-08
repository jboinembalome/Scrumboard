import { NgModule } from '@angular/core';
import { AddCommasPipe } from './add-commas.pipe';
import { EllipsisPipe } from './ellipsis.pipe';
import { LocalizedDatePipe } from './localized-date.pipe';

import { DatePipe } from '@angular/common';

export const PIPES = [AddCommasPipe, EllipsisPipe, LocalizedDatePipe];

@NgModule({
  declarations: PIPES,
  providers      : [
    DatePipe 
  ],
  exports: PIPES,
})
export class PipesModule {}
