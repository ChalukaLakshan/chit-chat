import { HttpClient, HttpHeaders } from "@angular/common/http";
import { EventEmitter, Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { Message } from "../models/message";
import { Observable } from "rxjs";
import { Options } from "selenium-webdriver";

@Injectable({
  providedIn: "root",
})
export class ChatService {
  private baseUrl = environment.baseApi;
  private hubConnection: HubConnection;
  messageReceived = new EventEmitter<Message>();

  constructor(private http: HttpClient) {
    this.init();
  }

  private init(): void {
    this.createConnection();
    this.startConnection();
  }

  private createConnection(): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.baseUrl}chat`)
      .build();
  }

  private startConnection(): void {
    this.hubConnection
      .start()
      .then(() => {
        console.log("Hub connection started");
        this.registerOnServerEvents();
      })
      .catch((error) => {
        console.log("Error while establishing connection, retrying...");
        setTimeout(function () {
          this.startConnection();
        }, 5000);
      });
  }

  private registerOnServerEvents(): void {
    this.hubConnection.on("MessageBroadcaster", (data: Message) => {
      this.messageReceived.emit(data);
    });
  }

  public sendMessage(message: Message): void {
    this.hubConnection.onclose(() => {
      this.init();
    });
    this.hubConnection.invoke("AddMessage", message);
  }

  public getHMessages(): Observable<Message[]> {
    let header = new HttpHeaders().append("Authorization", `Bearer ${sessionStorage.getItem('token')}`)
    return this.http.get<Message[]>(`${this.baseUrl}api/messages`, {headers: header});
  }

  public stopSignalR(): void {
    this.hubConnection.stop();
  }
}
