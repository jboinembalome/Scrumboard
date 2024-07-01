import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { IUser } from 'app/core/auth/models/user.model';
import { AuthService } from 'app/core/auth/services/auth.service';
import { BlouppyUtils } from 'app/shared/utils/blouppyUtils';
import { CardDetailDto, CommentCreationModel, CommentDto, CommentsService } from 'app/swagger';
import { CreateCommentCommand } from 'app/swagger/model/createCommentCommand';
import { AsyncPipe } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';
import { MatFormField } from '@angular/material/form-field';


@Component({
    selector: 'comment-add',
    templateUrl: './comment-add.component.html',
    standalone: true,
    imports: [FormsModule, ReactiveFormsModule, MatFormField, MatInput, MatIcon, AsyncPipe]
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
    const createCommentCommand: CommentCreationModel = {
      message: this.commentForm.get('message').value,
    };

    this._commentsService.apiCardsCardIdCommentsPost(this.card.id, createCommentCommand).subscribe(response => {
      this.commentAdded.emit(response.comment);
      this.commentForm.reset();
    });
  }
}
