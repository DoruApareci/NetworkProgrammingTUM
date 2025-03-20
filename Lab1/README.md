# Network Programming Lab 1 - TCP Chat Room

In this networking programming laboratory assignment, the objective was to develop a chat application using TCP. The solution comprises two distinct applications:
- **Server Application:** Accepts and manages multiple client connections, displays received messages, and broadcasts them to all connected clients.
- **Client Application:** Prompts the user to input text via the keyboard, sends the text to the server, and displays messages received from the server.

<p align="center"> <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab1/Images/image_1.png" alt="Main Image: Server and Clients Running"/> 
</p>

## Project Overview

The project is structured into four main components, each implemented as a separate project within the Visual Studio solution (VS):

- **Client.BL (Client Business Logic):**  
  Contains the necessary communication logic including interfaces, implementations, and additional logic to handle data exchange with the server.

- **Client.UI (Client User Interface):**  
  Provides the UI components that allow users to interact with the client business logic (Client.BL).

- **Server.BL (Server Business Logic):**  
  Contains the communication logic required for managing connections, receiving messages from clients, and broadcasting messages to all clients.

- **Server.UI (Server User Interface):**  
  Includes the UI components for the server, enabling it to display messages and manage client connections.

The projects are developed using **.Net**, **WPF**, **MVVM**, and **FodyWeavers**, ensuring a robust, maintainable, and scalable implementation.

## Assignment Description

**TCP Chat Application (Assignment #1)**

- **Purpose:**  
  To create a TCP-based chat application where:
  - The client sends a message entered by the user to the server.
  - The server displays the message and broadcasts it to all connected clients.
  - Multiple clients can send messages simultaneously.
  
- **Evaluation Criteria:**
  - Successful connection between client and server.
  - The server accepts concurrent client connections.
  - The client sends messages to the server, which then displays them.
  - The server is capable of rebroadcasting messages to all clients.
  - Both client and server can connect, disconnect, and transmit data without critical exceptions.

## How to Use

1. **Clone the Repository**

   ```bash
   git clone https://github.com/DoruApareci/NetworkProgrammingTUM.git
    ```
2. **Install Dependencies**

Ensure you have the required version of .NET and any other necessary dependencies installed.

3. **Open the Project**

Open the solution file (PRLab1.sln) in your preferred IDE (e.g., Visual Studio).

```bash
cd PRLab1.sln
start PRLab1.sln
```
4. **Build the Project**

Compile the projects using the .NET CLI or through Visual Studio.

```bash
dotnet build
```
5. **Run the Server Application**

Navigate to the server UI project directory and run the server. Configure the IP and port as needed.

```bash
cd Server.UI
dotnet run
```
5. **Run the Client Application**

In a separate terminal, navigate to the client UI project directory and run the client. Configure the IP, port, and username as required.

```
bash
cd ../Client.UI
dotnet run
```
6. **Start Chatting**

Once both the server and the client are running, you can start sending messages from the client which will be displayed on the server and rebroadcast to all connected clients.

## Screenshots

Below are some screenshots illustrating the application in action:

<p align="center"> <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab1/Images/Client_UI.png" alt="Client UI"/> </p> 
<p align="center"> <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab1/Images/Server_UI.png" alt="Server UI"/> </p> 

Contributing
Feel free to star the repository if you find it useful. Pull requests are welcome to help improve the project.
