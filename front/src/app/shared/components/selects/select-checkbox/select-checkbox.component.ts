import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatOption } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';

@Component({
    selector: 'blouppy-select-checkbox',
    templateUrl: './select-checkbox.component.html',
    styleUrls: ['./select-checkbox.component.scss'],
    standalone: true,
    imports: [MatSelect, MatOption]
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
