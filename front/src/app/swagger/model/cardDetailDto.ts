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
import { ActivityDto } from './activityDto';
import { AdherentDto } from './adherentDto';
import { ChecklistDto } from './checklistDto';
import { CommentDto } from './commentDto';
import { LabelDto } from './labelDto';

export interface CardDetailDto { 
    id?: number;
    name?: string;
    description?: string;
    suscribed?: boolean;
    dueDate?: Date;
    position?: number;
    listBoardId?: number;
    labels?: Array<LabelDto>;
    assignees?: Array<AdherentDto>;
    checklists?: Array<ChecklistDto>;
    comments?: Array<CommentDto>;
    activities?: Array<ActivityDto>;
}