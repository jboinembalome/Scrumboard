import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector     : 'blouppy-input-with-icon',
    templateUrl  : './input-with-icon.component.html',
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
