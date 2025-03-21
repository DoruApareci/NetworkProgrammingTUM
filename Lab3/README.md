# DNS App - DNS Resolver Tool

This laboratory assignment involves creating a DNS resolver application that allows users to resolve domain names to IP addresses (and vice versa) as well as change the DNS server used for resolution. The solution is implemented as a Visual Studio solution with two projects:

- **DNSTools:**  
  Contains the business logic (BL) for handling DNS resolution operations.
  
- **DNSApp.UI:**  
  Provides the user interface (UI) for interacting with the application.

<p align="center">
<img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab3/Images/DNSClient.png" alt="DNS Resolve Command"/>
</p>

The projects are developed using **.Net**, **WPF**, **MVVM**, and **FodyWeavers**, ensuring a modular, maintainable, and scalable solution.

## Assignment Description

The application reads two types of commands:

1. **resolve `<domain>` or resolve `<ip>`**  
   - When a domain is provided, the application displays the list of IP addresses assigned to that domain.
   - When an IP address is provided, the application shows the list of domains assigned to that IP.
   - By default, the system’s DNS server is used unless the user specifies an alternative.

2. **use dns `<ip>`**  
   - This command changes the DNS server used for the resolution operations described above.

### Evaluation Criteria

- The application can resolve a domain name to one or more IP addresses.
- The application can resolve an IP address to the corresponding domain(s).
- The application supports using a DNS server other than the one preset by the system.
- In cases where the DNS server cannot resolve the query, the application displays an appropriate error message.
- If the user specifies a non-existent DNS server IP, an error message is shown.

## How It Works

- **DNS Resolution:**  
  The core functionality in **DNSTools** handles both forward and reverse DNS lookup operations using the system’s DNS server by default or a user-defined DNS server when specified.

- **User Interface:**  
  The **DNSApp.UI** project provides a WPF-based interface where users can enter commands, view results, and receive feedback on errors or invalid inputs.

## How to Use

Clone, build and run the project from the repository. Then, type the command in the UI using the displayed patterns and press Enter; the response will be displayed in the UI.

For example:
- To resolve a domain or IP, type:
``` bash
resolve <domain_or_ip>
```

- To change the DNS server, type:
``` bash
use dns <ip>
```
## Screenshots

Below are some screenshots demonstrating the application's functionality:

<p align="center">
<img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab3/Images/DNSClient.png" alt="DNS Resolve Command"/>
</p>

<p align="center">
<img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab3/Images/DNSClient1.png" alt="Use DNS Command"/>
</p>

## Contributing

Feel free to star the repository if you find it useful. Pull requests and feedback are welcome to help improve the project.
