import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ColourDto } from 'src/app/swagger';

@Component({
    selector: 'blouppy-color-picker',
    templateUrl: './color-picker.component.html',
})
export class ColorPickerComponent {
    @Input() color: ColourDto = { colour: 'bg-white' };
    @Output() colorChange = new EventEmitter<ColourDto>();
    @Input() label: string = 'Select Color';
    @Input() placeholder: string = 'Pick a color';

    isOpen: boolean = false;
    colors: ColourDto[] =
        [
            { colour: 'bg-blue-500' },
            { colour: 'bg-yellow-500' },
            { colour: 'bg-red-500' },
            { colour: 'bg-indigo-500' },
            { colour: 'bg-rose-500' },
            { colour: 'bg-pink-500' },
            { colour: 'bg-purple-500' },
            { colour: 'bg-violet-500' },
            { colour: 'bg-orange-500' },
            { colour: 'bg-amber-500' },
            { colour: 'bg-green-500' },
            { colour: 'bg-teal-500' },
            { colour: 'bg-black' },
            { colour: 'bg-gray-500' },
            { colour: 'bg-white' }
        ];

    constructor() {
    }

    displayColor(color: ColourDto): string {
        let strColor: string;

        switch (color.colour) {
            case "bg-blue-500":
                strColor = "Blue";
            case "bg-yellow-500":
                strColor = "Yellow";
            case "bg-red-500":
                strColor = "Red";
            case "bg-indigo-500":
                strColor = "Indigo";
            case "bg-rose-500":
                strColor = "Rose";
            case "bg-pink-500":
                strColor = "Pink";
            case "bg-purple-500":
                strColor = "Purple";
            case "bg-violet-500":
                strColor = "Violet";
            case "bg-orange-500":
                strColor = "Orange";
            case "bg-amber-500":
                strColor = "Amber";
            case "bg-green-500":
                strColor = "Green";
            case "bg-teal-500":
                strColor = "Teal";
            case "bg-black":
                strColor = "Black";
            case "bg-gray-500":
                strColor = "Gray";
            case "bg-white":
                strColor = "White";
            default:
                strColor = color.colour;

                this.colorChange.emit(color);
                return strColor;
        }
    }
}
