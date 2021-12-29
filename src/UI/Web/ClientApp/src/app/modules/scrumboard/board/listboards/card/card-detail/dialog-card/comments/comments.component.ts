import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommentDto } from 'src/app/swagger';

@Component({
  selector: 'comments',
  templateUrl: './comments.component.html'
})
export class CommentsComponent {
  @Input() comments: CommentDto[];
  @Output() commentsUpdated = new EventEmitter<CommentDto[]>();

  constructor() {
  }

  removeComment(comment: CommentDto): void {
    this.comments.splice(this.comments.indexOf(comment), 1);

    this.commentsUpdated.emit(this.comments);
  }

  updateComment(comment: CommentDto): void {
    let index = this.comments.findIndex(c => c.id === comment.id);
    if (index < 0)
      this.comments[index] = comment;

    this.commentsUpdated.emit(this.comments);
  }
}
