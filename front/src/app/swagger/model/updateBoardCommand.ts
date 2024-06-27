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
import { BoardSettingDto } from './boardSettingDto';
import { ListBoardDto } from './listBoardDto';
import { TeamDto } from './teamDto';

export interface UpdateBoardCommand { 
    boardId?: number;
    name?: string;
    uri?: string;
    team?: TeamDto;
    boardSetting?: BoardSettingDto;
    listBoards?: Array<ListBoardDto>;
}