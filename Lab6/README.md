# Network Programming Lab 6 - NTP Client Application

This laboratory assignment involves creating a simple NTP client application that retrieves the correct time from an NTP server and adjusts it based on a user-provided geographical zone. The application is implemented as a Visual Studio solution with a single subproject: **NTPClientApp**.

The project is developed using **.Net** and **WPF** with an MVVM-inspired design for clarity and maintainability.

## Project Overview

The **NTPClientApp** contains the following key components:

- **MainWindow.xaml:**  
  Provides the user interface which includes:
  - A text field for entering the time zone (formatted as "GMT+X" or "GMT-X", where X is a number from 0 to 11).
  - A button to trigger the synchronization process.
  - A text block for displaying the adjusted local time.

- **MainWindow.cs:**  
  Contains the logic necessary to:
  - Validate the user input for the time zone.
  - Connect to an NTP server (`time.windows.com`) to retrieve the current UTC time.
  - Parse the user-provided time zone offset.
  - Calculate and display the local time based on the provided offset.

The key functionalities include:
- **Input Validation:**  
  Ensures the time zone is entered in the correct format ("GMT+X" or "GMT-X").
- **Network Time Retrieval:**  
  Uses a UDP socket to communicate with an NTP server and retrieve the current UTC time.
- **Time Calculation:**  
  Applies the user-specified time zone offset to the retrieved UTC time and displays the correct local time.

## Assignment Description

Develop an application that prompts the user to enter a geographical zone (in the format "GMT+X" or "GMT-X", where X is a digit from 0 to 11) and displays the exact local time for that zone. The application must validate the input format, retrieve the current time from an NTP server, and adjust the time according to the specified offset.

## Evaluation Criteria

- The application correctly retrieves the current UTC time from the NTP server.
- The application validates the time zone input format.
- The application correctly calculates and displays the local time based on the user-provided time zone offset.
- The UI is clear and user-friendly.

## How to Use

Clone, build, and run the project from the repository. Once the application is running, use the provided UI to enter the desired time zone and click the "Sync Time" button to display the local time for that zone.

## Application UI
<p align="center"> <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab6/Images/NTP.png" alt="NTP Client App UI"/> </p>

## Contributing
Feel free to star the repository if you find it useful. Pull requests and suggestions are welcome to help improve the project.