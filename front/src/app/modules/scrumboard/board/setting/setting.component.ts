import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';
import { BoardSettingDto, ColourDto } from 'app/swagger';

@Component({
    selector: 'scrumboard-board-setting',
    templateUrl: './setting.component.html',
    styleUrls: ['./setting.component.scss']
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

    updateCardCoverImage(): void {
        this.boardSetting.cardCoverImage = !this.boardSetting.cardCoverImage;

        this.boardSettingChange.emit(this.boardSetting);
    }

    updateSubscribed(): void {
        this.boardSetting.subscribed = !this.boardSetting.subscribed;

        this.boardSettingChange.emit(this.boardSetting);
    }
}

