import { Pipe, PipeTransform } from '@angular/core';
import { ColourDto } from 'src/app/swagger';

@Pipe({ name: 'stringColor' })
export class StringColorPipe implements PipeTransform {
  /**
   * Transform
   *
   * @param {ColourDto} color
   * @param {string} searchText
   * @returns {string}
   */
  transform(color: ColourDto, stringColor: string = null): string {
    if (!color) {
      return '';
    }

    if (stringColor) {
        console.log("toto")
      return stringColor;
    }

   return this.formatColor(color);
  }

  private formatColor(color: ColourDto): string {
    switch (color.colour) {
        case "bg-blue-500":
            return "Blue";
        case "bg-yellow-500":
          return "Yellow";
        case "bg-red-500":
          return "Red";
        case "bg-indigo-500":
            return "Indigo";
        case "bg-rose-500":
            return "Rose";
        case "bg-pink-500":
            return "Pink";
        case "bg-purple-500":
            return "Purple";
        case "bg-violet-500":
            return "Violet";
        case "bg-orange-500":
            return "Orange";
        case "bg-amber-500":
            return "Amber";
        case "bg-green-500":
            return "Green";
        case "bg-teal-500":
            return "Teal";
        case "bg-gray-900":
            return "Black";
        case "bg-gray-500":
            return "Gray";
        case "bg-white":
            return "White";
        default:
            return color.colour;
    }
}
}