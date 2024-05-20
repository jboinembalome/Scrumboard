import { Pipe, PipeTransform } from '@angular/core';
import { BlouppyUtils } from '../utils/blouppyUtils';

@Pipe({ name: 'initial' })
export class InitialPipe implements PipeTransform {

    /**
    * Transform
    *
    * @param {string} fullName
    * @returns {any}
    */
    transform(fullName: string): string {
        return BlouppyUtils.getInitialsByFullName(fullName);
    };
}
