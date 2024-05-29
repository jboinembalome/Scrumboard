import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommentDto, CommentsService } from 'app/swagger';
import { UpdateCommentCommand } from 'app/swagger/model/updateCommentCommand';

@Component({
  selector: 'comments',
  templateUrl: './comments.component.html'
})
export class CommentsComponent {
  @Input() comments: CommentDto[];
  @Output() commentsUpdated = new EventEmitter<CommentDto[]>();

  constructor(private _commentsService: CommentsService) {
  }

  removeComment(comment: CommentDto): void {
    this._commentsService.apiCommentsIdDelete(comment.id).subscribe(() => {
      this.comments.splice(this.comments.indexOf(comment), 1);
      this.commentsUpdated.emit(this.comments);  
    });
  }

  updateComment(comment: CommentDto): void {
    const updateCommentCommand: UpdateCommentCommand = {
      id: comment.id,
      message: comment.message,
    };

    this._commentsService.apiCommentsIdPut(comment.id, updateCommentCommand).subscribe(response => {
      var index = this.comments.findIndex(c => c.id === response.comment.id);
      if (index >= 0) {
        this.comments[index] = response.comment;
        this.commentsUpdated.emit(this.comments);
      }
    });


  }
}
