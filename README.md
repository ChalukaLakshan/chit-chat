# Steps to Run the Chit-Chat Application
1. Clone the Project from the repository.

2. Open the ChitChatWebSolution from the Visual Studio 2019.

3. Change the connection string (DefaultConnection) in appsettings.Development.json.

4. Open the Package Manager Console and select the Default project as Chat.Data

5. Now need to update the database using the code FIRST approach (Please run the following Commands respectively).

    - add-migration command will create folder as Migrations in Chat.Data project.
    - update-databse command will be responsible for creating the tables accordingly in the database.
    
6. Open the Client application from the Visual Studio Code (chit-chat/ChatClient/chat-client)

7. Open the terminal in the Visual Studio code and run (npm install) command to create the node modules.

8. Run the ChitChat.Api from the Visual Studio.

9. use the ng serve command to run the client application. ( should run on http://localhost:4200).
