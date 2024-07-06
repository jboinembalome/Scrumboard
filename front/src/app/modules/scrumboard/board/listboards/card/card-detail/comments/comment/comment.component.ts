import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from 'app/core/auth/services/auth.service';
import { CommentDto } from 'app/swagger';
import { DatePipe } from '@angular/common';
import { MatInput } from '@angular/material/input';
import { MatFormField } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatMenuTrigger, MatMenu } from '@angular/material/menu';
import { MatIconButton } from '@angular/material/button';

@Component({
    selector: 'comment',
    templateUrl: './comment.component.html',
    standalone: true,
    imports: [MatIconButton, MatMenuTrigger, MatIcon, MatMenu, FormsModule, ReactiveFormsModule, MatFormField, MatInput, DatePipe]
})
export class CommentComponent implements OnInit {
  @Input() comment: CommentDto;
  @Output() commentUpdated = new EventEmitter<CommentDto>();
  @Output() commentRemoved = new EventEmitter<CommentDto>();
  
  commentForm: UntypedFormGroup;

  canModify: boolean = false;
  editCommentCliked: boolean;

  urlAvatar: string = location.origin + "/api/users/avatar/";

  constructor(
    private _authService: AuthService, 
    private _formBuilder: UntypedFormBuilder) {
  }

  ngOnInit(): void {
    // Load the current user's data to check if we can update or remove the comment
    this._authService.getUser().subscribe(user => {
      if (this.comment.user.id === user.id)
        this.canModify = true;
      else
        this.canModify = false;
    })

    // Prepare the comment form
    this.commentForm = this._formBuilder.group({
      message: [this.comment.message],
    });
  }

  updateComment(): void {
    this.comment.message = this.commentForm.get('message').value;
    //this.comment.modifiedDate = new Date();
    this.commentUpdated.emit(this.comment);

    this.commentForm.reset({message : this.comment.message});
    this.editCommentCliked = false;
  }
}
