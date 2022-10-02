import { Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { Router } from "@angular/router";
import { Message } from "src/app/models/message";
import { User } from "src/app/models/user";
import { ChatService } from "src/app/services/chat.service";
import { UserService } from "src/app/services/user.service";

@Component({
  selector: "app-inbox",
  templateUrl: "./inbox.component.html",
  styleUrls: ["./inbox.component.css"],
})
export class InboxComponent implements OnInit, OnDestroy {
  public messages: Message[] = [];
  public users: User[] = [];
  public loggedUsername: string;
  public textareaValue: string;
  public searchText: string = "";

  constructor(
    private router: Router,
    private chatService: ChatService,
    private snackBar: MatSnackBar,
    private userService: UserService
  ) {}

  ngOnDestroy(): void {
    this.chatService.stopSignalR();
  }

  ngOnInit(): void {
    this.textareaValue = "";

    this.loggedUsername = sessionStorage.getItem("username");
    this.getSignalRMessage();

    this.chatService.getHMessages().subscribe(
      (response) => {
        if (response && response.length) {
          this.messages = response;
        }
      },
      (error) => {
        if (error && error.status == 401) {
          this.snackBar.open("ERROR", "Session expired. Please login again.");
          this.logout();
        }

        if (error && error.error && error.error.message) {
          this.snackBar.open("ERROR", error.error.message);
        }

        console.log(error);
      }
    );

    this.userService.getAllUsers().subscribe(
      (response) => {
        if (response && response.length) {
          this.users = response;

          let index = this.users.findIndex(
            (i) => i.username === this.loggedUsername
          );

          if (typeof index) {
            this.users.splice(index, 1);
          }
        }
      },
      (error) => {
        if (error && error.status == 401) {
          this.snackBar.open("ERROR", "Session expired. Please login again.");
          this.logout();
        }

        if (error && error.error && error.error.message) {
          this.snackBar.open("ERROR", error.error.message);
        }

        console.log(error);
      }
    );
  }

  private getSignalRMessage(): void {
    this.chatService.messageReceived.subscribe((message: Message) => {
      this.messages.push(message);
    });
  }

  public sendMessage(): void {
    if (this.textareaValue) {
      let message = new Message();
      message.fromUsername = this.loggedUsername;
      message.date = new Date();
      message.messageText = this.textareaValue;

      this.chatService.sendMessage(message);
      this.textareaValue = "";
    }
  }

  public onUserSelect(authorId) {
    alert(authorId);
  }

  public searchUser(): void {
    if (this.searchText) {
      this.userService.getUserByName(this.searchText).subscribe(
        (response) => {
          if (response && response.username) {
            let index = this.users.findIndex(
              (i) => i.username === response.username
            );

            if (typeof index) {
              this.users.splice(index, 1);
              this.users.unshift(response);
            } else {
              this.users.unshift(response);
            }
            this.snackBar.open("Well done! Inbox Updated", "Dance");
          }
        },
        (error) => {
          if (error && error.status == 401) {
            this.snackBar.open("ERROR", "Session expired. Please login again.");
            this.logout();
          }

          if (error && error.error && error.error.message) {
            this.snackBar.open("ERROR", error.error.message);
          }
          console.log(error);
        }
      );
    }
  }

  public logout(): void {
    window.sessionStorage.removeItem("token");
    window.sessionStorage.removeItem("username");
    this.router.navigate([""]);
  }
}
