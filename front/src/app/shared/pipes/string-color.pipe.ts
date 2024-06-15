import { Pipe, PipeTransform } from '@angular/core';
import { ColourDto } from 'app/swagger';
import { BlouppyUtils } from '../utils/blouppyUtils';

@Pipe({
    name: 'stringColor',
    standalone: true
})
export class StringColorPipe implements PipeTransform {
  /**
   * Transform
   *
   * @param {ColourDto} color
   * @param {string} searchText
   * @returns {string}
   */
  transform(color: ColourDto, stringColor: string = null): string {
    if (!color)
      return '';

    if (stringColor)
      return stringColor;

   return BlouppyUtils.formatColor(color);
  }
}