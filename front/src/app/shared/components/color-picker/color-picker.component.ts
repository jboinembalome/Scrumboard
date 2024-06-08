import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ColourDto } from 'app/swagger';
import { StringColorPipe } from '../../pipes/string-color.pipe';
import { MatMenuTrigger, MatMenu } from '@angular/material/menu';
import { FormsModule } from '@angular/forms';
import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'blouppy-color-picker',
    templateUrl: './color-picker.component.html',
    standalone: true,
    imports: [
        MatIcon,
        FormsModule,
        MatMenuTrigger,
        MatMenu,
        StringColorPipe,
    ],
})
export class ColorPickerComponent {
    @Input() color: ColourDto = { colour: 'bg-white' };
    @Output() colorChange = new EventEmitter<ColourDto>();
    @Input() label: string = 'Select Color';
    @Input() placeholder: string = 'Pick a color';

    isOpen: boolean = false;
    colors: ColourDto[] =
        [
            { colour: 'bg-blue-800/30' },
            { colour: 'bg-yellow-800/30' },
            { colour: 'bg-red-800/30' },
            { colour: 'bg-indigo-800/30' },
            { colour: 'bg-rose-800/30' },
            { colour: 'bg-pink-800/30' },
            { colour: 'bg-purple-800/30' },
            { colour: 'bg-violet-800/30' },
            { colour: 'bg-orange-800/30' },
            { colour: 'bg-amber-800/30' },
            { colour: 'bg-green-800/30' },
            { colour: 'bg-teal-800/30' },
            { colour: 'bg-gray-800/30' }
        ];

    constructor() {
    }

    updateColor(color: ColourDto): void {
        this.color = color;
        this.colorChange.emit(this.color);
    }
}
