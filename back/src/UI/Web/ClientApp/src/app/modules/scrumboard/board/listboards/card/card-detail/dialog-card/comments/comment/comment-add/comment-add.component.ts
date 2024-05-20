import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { IUser } from 'src/app/core/auth/models/user.model';
import { AuthService } from 'src/app/core/auth/services/auth.service';
import { BlouppyUtils } from 'src/app/shared/utils/blouppyUtils';
import { CardDetailDto, CommentDto, CommentsService } from 'src/app/swagger';
import { CreateCommentCommand } from 'src/app/swagger/model/createCommentCommand';


@Component({
  selector: 'comment-add',
  templateUrl: './comment-add.component.html'
})
export class CommentAddComponent implements OnInit {
  commentForm: FormGroup;
  @Input() card: CardDetailDto;
  @Output() commentAdded = new EventEmitter<CommentDto>();

  currentUser: Observable<IUser>;

  constructor(
    private _authService: AuthService,
    private _commentsService: CommentsService, 
    private _formBuilder: FormBuilder) {
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
