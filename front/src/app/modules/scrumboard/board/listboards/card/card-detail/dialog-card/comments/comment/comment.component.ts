import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { AuthService } from 'src/app/core/auth/services/auth.service';
import { CommentDto } from 'src/app/swagger';

@Component({
  selector: 'comment',
  templateUrl: './comment.component.html'
})
export class CommentComponent implements OnInit {
  @Input() comment: CommentDto;
  @Output() commentUpdated = new EventEmitter<CommentDto>();
  @Output() commentRemoved = new EventEmitter<CommentDto>();
  
  commentForm: UntypedFormGroup;

  canModify: boolean = false;
  editCommentCliked: boolean;

  urlAvatar: string = location.origin + "/api/adherents/avatar/";

  constructor(
    private _authService: AuthService, 
    private _formBuilder: UntypedFormBuilder) {
  }

  ngOnInit(): void {
    // Load the current user's data to check if we can update or remove the comment
    this._authService.getUser().subscribe(user => {
      if (this.comment.adherent.identityId === user.id)
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
