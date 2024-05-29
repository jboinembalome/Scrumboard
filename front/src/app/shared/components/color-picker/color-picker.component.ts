import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ColourDto } from 'app/swagger';

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
            { colour: 'bg-gray-500' }
        ];

    constructor() {
    }

    updateColor(color: ColourDto): void {
        this.color = color;
        this.colorChange.emit(this.color);
    }
}
