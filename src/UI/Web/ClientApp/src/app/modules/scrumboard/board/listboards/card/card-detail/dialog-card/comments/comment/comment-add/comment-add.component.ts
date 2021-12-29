import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/core/auth/services/auth.service';
import { BlouppyUtils } from 'src/app/shared/utils/blouppyUtils';
import { CommentDto } from 'src/app/swagger';


@Component({
  selector: 'comment-add',
  templateUrl: './comment-add.component.html'
})
export class CommentAddComponent implements OnInit {
  commentForm: FormGroup;
  userName: string;
  @Output() commentAdded = new EventEmitter<CommentDto>();

  constructor(
    private _authService: AuthService, 
    private _formBuilder: FormBuilder) {
  }

  ngOnInit(): void {
    this._authService.getUser().subscribe(user => {
      this.userName = user.name;
    })

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
    const comment: CommentDto = {
      message: this.commentForm.get('message').value,
    };

    this.commentAdded.emit(comment);
    this.commentForm.reset();
  }
}
