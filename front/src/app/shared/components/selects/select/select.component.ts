import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatOption } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';

@Component({
    selector: 'blouppy-select',
    templateUrl: './select.component.html',
    styleUrls: ['./select.component.scss'],
    standalone: true,
    imports: [MatSelect, MatOption]
})
export class SelectComponent {
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
