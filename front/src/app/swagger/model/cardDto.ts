/**
 * Scrumboard API
 * API for accessing Scrumboard project data.
 *
 * OpenAPI spec version: v1
 * Contact: jboinembaome@gmail.com
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */
import { LabelDto } from './labelDto';
import { UserDto } from './userDto';

export interface CardDto { 
    id?: number;
    name?: string;
    suscribed?: boolean;
    dueDate?: Date;
    position?: number;
    listBoardId?: number;
    labels?: Array<LabelDto>;
    assignees?: Array<UserDto>;
    attachmentsCount?: number;
    checklistItemsCount?: number;
    checklistItemsDoneCount?: number;
}