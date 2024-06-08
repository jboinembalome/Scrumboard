import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'boardFilter',
    standalone: true
})
export class BoardsFilterPipe implements PipeTransform {
  /**
   * Transform
   *
   * @param {any[]} boards
   * @param {string} searchText
   * @returns {any[]}
   */
  transform(boards: any[], searchText: string): any[] {
    if (!boards)
      return [];
    
    if (!searchText)
      return boards;

    searchText = searchText.toLocaleLowerCase();

    return boards.filter(board => board.name.toLocaleLowerCase().includes(searchText));
  }
}