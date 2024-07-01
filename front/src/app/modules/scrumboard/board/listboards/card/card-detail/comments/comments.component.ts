import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommentDto, CommentsService } from 'app/swagger';
import { UpdateCommentCommand } from 'app/swagger/model/updateCommentCommand';
import { CommentComponent } from './comment/comment.component';

@Component({
    selector: 'comments',
    templateUrl: './comments.component.html',
    standalone: true,
    imports: [CommentComponent]
})
export class CommentsComponent {
  @Input() comments: CommentDto[];
  @Input() cardId: number;
  @Output() commentsUpdated = new EventEmitter<CommentDto[]>();

  constructor(private _commentsService: CommentsService) {
  }

  removeComment(comment: CommentDto): void {
    this._commentsService.apiCardsCardIdCommentsIdDelete(this.cardId, comment.id).subscribe(() => {
      this.comments.splice(this.comments.indexOf(comment), 1);
      this.commentsUpdated.emit(this.comments);  
    });
  }

  updateComment(comment: CommentDto): void {
    const updateCommentCommand: UpdateCommentCommand = {
      id: comment.id,
      message: comment.message,
    };

    this._commentsService.apiCardsCardIdCommentsIdPut(this.cardId, comment.id, updateCommentCommand).subscribe(response => {
      var index = this.comments.findIndex(c => c.id === response.comment.id);
      if (index >= 0) {
        this.comments[index] = response.comment;
        this.commentsUpdated.emit(this.comments);
      }
    });


  }
}
