# Network Programming Lab 2 - UDP Chat Application

This laboratory assignment involves creating a UDP-based chat application that works within a network segment. The application provides both a general chat channel—where multicast messages are received and displayed to all participants—and private conversations with individual network participants, which are not shown in the general channel.

<p align="center">
  <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab2/Images/General.png" alt="General Chat UI"/>
</p>

## Project Overview

This project is implemented as a single Visual Studio solution containing one project: **UDPChat**. The project integrates both Business Logic (BL) and User Interface (UI) components within the same project structure.

### Project Structure

- **Models:**  
  Contains the data models used by the application.
  - **MessageType (enum):**  
    Defines the type of messages (e.g., Broadcast, Private).
  - **ActionType (enum):**  
    Defines actions such as None, Authorize, Authorized, and Deauthorize.
  - **Friend (class):**  
    Represents a chat participant with properties for Username, IP address, and a list of Messages.

- **Views:**  
  Contains the visual layout of the application.
  - **ChatView (UserControl):**  
    Serves as the reference for a selected chat, including both the visual layout (.xaml) and its corresponding ViewModel (.cs)
  - **MainWindow (Window):**  
    Contains the complete application logic and serves as the primary UI (defined in MainWindow.xaml with code-behind in MainWindow.cs)

### Message Formatting
Messages exchanged within the application use a custom formatting pattern with the delimiter ">_<". For example, in the method for receiving messages, the data is split by this separator to process different message parts (e.g., message type, action, content, etc.):

``` csharp
private void RecieveMessage()
{
    try
    {
        EndPoint receiveEndPoint = new IPEndPoint(IPAddress.Any, 0);
        while (IsListening)
        {
            byte[] receivedBytes = new byte[1024];
            socket.ReceiveFrom(receivedBytes, ref receiveEndPoint);
            string[] response = Encoding.UTF8.GetString(receivedBytes).Split(">_<");

            if (response[0] == "BROADCAST")
            {
                if (response[1] == "Authorize")
                {
                    AddFriend(new Friend() { IP = ((IPEndPoint)receiveEndPoint).Address, Username = response[2].Replace("\0", string.Empty), Messages = new() });
                    SendMessage(MessageType.Private, ActionType.Authorized, "", Friends.IndexOf(Friends.Where(x => x.IP == ((IPEndPoint)receiveEndPoint).Address).First()));
                    continue;
                }
                else if (response[1] == "Deauthorize")
                {
                    RemoveFriend(((IPEndPoint)receiveEndPoint).Address);
                    continue;
                }
                else
                {
                    var username = FriendUsername(((IPEndPoint)receiveEndPoint).Address);
                    lock (Friends[0].Messages)
                    {
                        Friends[0].Messages.Add($"{username}: {response[2]}"); // log general message
                        SelectedChat.UpdateUI();
                    }
                    continue;
                }
            }
            else if (response[0] == "PRIVATE")
            {
                if (response[1] == "Authorized")
                {
                    if (Username != response[2].Replace("\0", string.Empty) && !Friends.Exists(c => c.Username == response[2].Replace("\0", string.Empty)))
                        AddFriend(new Friend() { IP = ((IPEndPoint)receiveEndPoint).Address, Username = response[2].Replace("\0", string.Empty), Messages = new() });
                    continue;
                }
                var fr = Friends.First(x => x.IP.ToString() == ((IPEndPoint)receiveEndPoint).Address.ToString());
                lock (fr.Messages)
                {
                    fr.Messages.Add($"{fr.Username}: {response[2]}"); // log private message
                    SelectedChat.UpdateUI();
                }
            }
        }
    }
    catch (Exception e)
    {
        MessageBox.Show(e.Message);
    }
}
```
## Assignment Description
**UDP Chat Application (Assignment #2)**

- **Purpose:**
  Develop a UDP chat application that:
  - Uses a UDP socket to transmit and receive messages.
  - Allows sending messages to a specific IP address (unicast).
  - Supports broadcasting messages to a general channel.
  - Enables private messaging between users.
  - Processes and displays messages appropriately based on their type.
  - Handles exceptions during data transmission gracefully.
- **Evaluation Criteria:**
  - Creation of a UDP socket capable of sending and receiving messages.
  - The client can send messages to a specific IP address.
  - The client can receive messages from a specific IP address.
  - The client can transmit messages on the general channel.
  - The client can receive and display messages from the general channel.
  - Exceptions during data transmission are properly handled.


## Technologies Used
  - .Net
  - WPF
  - MVVM
  - FodyWeavers


## How to Use
Clone, Build and Run the project from repository

Use the application to send messages in the general channel and to communicate privately with other network participants. Messages will be formatted using the custom pattern and processed accordingly.

## Screenshots
Below are some screenshots illustrating various aspects of the application:

<p align="center"> <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab2/Images/DM.png" alt="Private Chat (Unicast)"/> </p> 

<p align="center"> <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab2/Images/General.png" alt="General Chat (Broadcast)"/> </p> 

<p align="center"> <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab2/Images/SelfMessage.png" alt="Self Message Chat"/> </p>

## Contributing
Feel free to star the repository if you find it useful. Pull requests are welcome to help improve the project.