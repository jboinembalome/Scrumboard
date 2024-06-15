import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatIcon } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

@Component({
    selector: 'blouppy-input-with-icon',
    templateUrl: './input-with-icon.component.html',
    standalone: true,
    imports: [MatIcon, MatInputModule, FormsModule],
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
