import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { IUser } from 'app/core/auth/models/user.model';
import { AuthService } from 'app/core/auth/services/auth.service';
import { BlouppyUtils } from 'app/shared/utils/blouppyUtils';
import { CardDetailDto, CommentDto, CommentsService } from 'app/swagger';
import { CreateCommentCommand } from 'app/swagger/model/createCommentCommand';


@Component({
  selector: 'comment-add',
  templateUrl: './comment-add.component.html'
})
export class CommentAddComponent implements OnInit {
  commentForm: UntypedFormGroup;
  @Input() card: CardDetailDto;
  @Output() commentAdded = new EventEmitter<CommentDto>();

  currentUser: Observable<IUser>;

  constructor(
    private _authService: AuthService,
    private _commentsService: CommentsService, 
    private _formBuilder: UntypedFormBuilder) {
  }

  ngOnInit(): void {
    this.currentUser = this._authService.getUser();

    // Prepare the comment form
    this.commentForm = this._formBuilder.group({
      message: [''],
    });

    // Fill the form
    this.commentForm.setValue({
      message: '',     
    });
  }

  addComment(): void {
    const createCommentCommand: CreateCommentCommand = {
      message: this.commentForm.get('message').value,
      cardId: this.card.id,
    };

    this._commentsService.apiCommentsPost(createCommentCommand).subscribe(response => {
      this.commentAdded.emit(response.comment);
      this.commentForm.reset();
    });
  }
}
