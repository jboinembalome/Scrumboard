import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';
import { BoardSettingDto, ColourDto } from 'app/swagger';
import { ToggleComponent } from '../../../../shared/components/toggle/toggle.component';
import { ColorPickerComponent } from '../../../../shared/components/color-picker/color-picker.component';
import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'scrumboard-board-setting',
    templateUrl: './setting.component.html',
    styleUrls: ['./setting.component.scss'],
    standalone: true,
    imports: [MatIcon, ColorPickerComponent, ToggleComponent]
})
export class SettingComponent {
    @Input() boardSetting: BoardSettingDto;
    @Output() boardSettingChange = new EventEmitter<BoardSettingDto>();
    @Output() closeSettingPanel = new EventEmitter<void>();

    settingForm: UntypedFormGroup;
    ColourDto: ColourDto;

    constructor() {
    }

    updateColor(color: ColourDto): void {
        this.boardSetting.colour = color;

        this.boardSettingChange.emit(this.boardSetting);
    }
}

