# Network Programming Lab 1 - TCP Chat Room

In this networking programming laboratory assignment, students are tasked with creating two applications. One application will function as a network server, while the other will serve as a client connecting to the server. The client application prompts the user to input text from the keyboard and sends it to the server. The server displays this message in its window and then broadcasts it to all connected clients, including the one that originally sent the message. Clients have the ability to transmit multiple messages.

Feel free to **star** the repository if you find it useful. **Pull requests are welcome too**.

## Project Description

The NPLab1 solution is a networking programming project that involves creating a client-server application for a chat system using TCP. The project structure is organized into four main components:

### Client.BL (Client Business Logic):

- **ClientBL.cs:** Contains the implementation of the client business logic, responsible for handling communication with the server.

- **IClientLogger.cs:** Interface defining logging functionality for the client.
### Client.UI (Client User Interface):

- **ClientApp.cs:** Contains the client application logic, including user input handling and interaction with the Client.BL.

- **Program.cs:** Entry point for the client application.

### Server.BL (Server Business Logic):
- **IServerLogger.cs:** Interface defining logging functionality for the server.

- **ServerBL.cs:** Contains the implementation of the server business logic, managing client connections and message broadcasting.

### Server.UI (Server User Interface):
- **Program.cs:** Entry point for the server application.

- **ServerApp.cs:** Contains the server application logic, handling incoming client connections and messages.


## How to Use

1. Clone the repository to your local machine.

        git clone https://github.com/DoruApareci/NetworkProgrammingTUM.git
2. Install the necessary dependencies.
3. Open the project in your preferred IDE.

        cd NPLab1
        start NPLab1.sln
4. Compile the projects.

        dotnet build
5. Run the server first, configure the IP and port.

       cd Server.UI
       dotnet run
6. Run the client, configure the IP, port, and username.

        cd ../Client.UI
        dotnet run
7. Start chating.

## Images

Server UI for server administration and chat logging:

<p align="center">
  <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/NPLab1/images/Server.png" alt="Server UI"/>
</p>

Client 1 UI, for chating with other users:


<p align="center">
  <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/NPLab1/images/Client1.png" alt="Client 1"/>
</p>

Client 2 UI, for chating with other users:

<p align="center">
  <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/NPLab1/images/Client2.png" alt="Client 2"/>
</p>

Please refer to the images for visual references throughout the documentation.