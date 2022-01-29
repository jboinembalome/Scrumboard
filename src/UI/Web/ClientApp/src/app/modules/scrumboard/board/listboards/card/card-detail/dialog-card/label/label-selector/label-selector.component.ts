import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ColourDto, LabelDto } from 'src/app/swagger';

@Component({
  selector: 'label-selector',
  templateUrl: './label-selector.component.html'
})
export class LabelSelectorComponent {

  @Input() selectedLabels: LabelDto[];
  @Input() labels: LabelDto[];
  @Output() labelUpdated = new EventEmitter<LabelDto[]>();

  constructor() {
  }

  hasLabel(label: LabelDto): boolean {
    return !!this.selectedLabels.find(l => l.name === label.name);
  }

  toggleLabel(label: LabelDto, checked: boolean): void {
    if (checked)
      this.addLabelToCard(label);
    else
      this.removeLabelFromCard(label);

    this.labelUpdated.emit(this.selectedLabels);
  }

  updateLabel(label: LabelDto): void {
    const updatedLabel = this.selectedLabels.find(l => l.id === label.id);
    updatedLabel.name = label.name;
    updatedLabel.colour = label.colour;
    console.log(updatedLabel);
    console.log(this.selectedLabels);

    this.labelUpdated.emit(this.selectedLabels);
  }

  deleteLabel(label: LabelDto): void {
    this.selectedLabels = this.selectedLabels.filter(l => l.id !== label.id);

    this.labelUpdated.emit(this.selectedLabels);
  }

  /**
  * Tracks by function for ngFor loops.
  *
  * @param index
  * @param item
  */
  trackByFn(index: number, item: any): any {
    return item.id || index;
  }

  private addLabelToCard(label: LabelDto): void {
    this.selectedLabels.unshift(label);
  }

  private removeLabelFromCard(label: LabelDto): void {
    const index = this.selectedLabels.findIndex(l => l.id === label.id);

    if (index >= 0)
      this.selectedLabels.splice(index, 1);
  }
}
