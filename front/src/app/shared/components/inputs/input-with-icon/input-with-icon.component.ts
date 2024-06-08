import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'blouppy-input-with-icon',
    templateUrl: './input-with-icon.component.html',
    standalone: true,
    imports: [MatIcon, FormsModule],
})
export class InputWithIconComponent {
    @Input() key: string = '';
    @Input() value: string = '';
    @Output() valueChange = new EventEmitter<string>();
    @Input() label: string = '';
    @Input() icon: string = '';
    @Input() placeholder: string = '';
    
    constructor() {
    }
}
