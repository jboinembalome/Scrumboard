import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector     : 'blouppy-select-checkbox',
    templateUrl  : './select-checkbox.component.html',
    styleUrls: ['./select-checkbox.component.scss']

})
export class SelectCheckboxComponent {
    @Input() dataSource: any[];
    @Input() label: string;
    @Input() labelKey: string;
    @Input() value: any;
    @Output() valueChange = new EventEmitter<any>();
    @Output() selectionChange = new EventEmitter<any>();
    @Input() valueKey: string;

    constructor() {
    }
}
