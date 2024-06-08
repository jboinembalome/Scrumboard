import { Pipe, PipeTransform } from '@angular/core';
import { ColourDto } from 'app/swagger';

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

   return this.formatColor(color);
  }

  private formatColor(color: ColourDto): string {
    switch (color.colour) {
        case "bg-blue-800/30":
            return "Blue";
        case "bg-yellow-800/30":
          return "Yellow";
        case "bg-red-800/30":
          return "Red";
        case "bg-indigo-800/30":
            return "Indigo";
        case "bg-rose-800/30":
            return "Rose";
        case "bg-pink-800/30":
            return "Pink";
        case "bg-purple-800/30":
            return "Purple";
        case "bg-violet-800/30":
            return "Violet";
        case "bg-orange-800/30":
            return "Orange";
        case "bg-amber-800/30":
            return "Amber";
        case "bg-green-800/30":
            return "Green";
        case "bg-teal-800/30":
            return "Teal";
        case "bg-gray-800/30":
            return "Gray";
        default:
            return color.colour;
    }
}
}