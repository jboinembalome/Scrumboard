import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'plural',
    standalone: true
})
export class PluralPipe implements PipeTransform {
  /**
    * Transform
    *
    * @param {number} input
    * @param {string} customPluralForm
    * @returns {string}
    */
  transform(input: number, customPluralForm: string = "s"): string {
    return input > 1 ? customPluralForm : "";
  }
}