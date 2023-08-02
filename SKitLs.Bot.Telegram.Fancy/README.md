# SKitLs.Bots.Telegram.AdvancedMessages ![Static Badge](https://img.shields.io/badge/Follow%20GitHub%20-%20black?logo=github&link=https%3A%2F%2Fgithub.com%2FSargeras02%2FSKitLs.Bots.Telegram.git) ![GitHub](https://img.shields.io/github/license/Sargeras02/SKitLs.Bots.Telegram) ![Nuget](https://img.shields.io/nuget/v/SKitLs.Bots.Telegram.AdvancedMessages) [![CodeFactor](https://www.codefactor.io/repository/github/sargeras02/skitls.bots.telegram/badge)](https://www.codefactor.io/repository/github/sargeras02/skitls.bots.telegram)

An extension project built upon the SKitLs.Bots.Telegram.Core Framework.

Offers enhanced methods for sending messages in Telegram bots.

## Description

SKitLs.Bots.Telegram.AdvancedMessages serves as an extension to the SKitLs.Bots.Telegram Framework,
enriching the message-sending functionality in Telegram bots.
The primary objective of this extension is to offer improved methods for message creation and delivery,
thus enabling more flexible and interactive communication with users.

The key functionalities of this extension include:

1. Advanced Delivery:

    The `AdvancedDeliverySystem` is an enhanced implementation of the `IDeliveryService` interface,
    designed to facilitate seamless interaction with message classes within the library.
    Replacing the `DefaultDeliverySystem`, this feature introduces a more sophisticated and versatile approach to message handling.

2. Menus:

    The extension empowers the creation of messages with integrated inline menus, allowing users to interact with the bot
    seamlessly without leaving the chat window. Also you can augment messages with Reply Markups, presenting users with
    a set of buttons to choose from, leading to prompt responses and actions, thereby streamlining interactions.

3. Message Formatting (WIP):

    SKitLs.Bots.Telegram.AdvancedMessages provides the ability to format text messages with various styles, such as bold, italic,
    and stroke text. This enables more expressive and comprehensible presentation of textual information.

As part of the expansion plan, developers aim to introduce functionality for sending audio files, media, and documents.
This enhancement will facilitate more efficient exchange of diverse content types between bots and users,
elevating the quality and diversity of communication.

SKitLs.Bots.Telegram.AdvancedMessages is committed to empowering Telegram bot developers with tools to create more interactive,
informative, and user-friendly messages, thereby enhancing the overall user experience and satisfaction.

## Setup

### Requirements

- Telegram.Bot 19.0.0 or higher
- SKitLs.Bots.Telegram.Core 2.0.0 or higher
- SKitLs.Bots.Telegram.ArgedInteractions 1.3.0 or higher

Before running the project, please ensure that you have the following dependencies installed and properly configured in your development environment.

### Installation

1. Using Terminal Command:
    
    To install the project using the terminal command, follow these steps:

    1. Open the terminal or command prompt.
    2. Run command:
    
    ```
    dotnet add package SKitLs.Bots.Telegram.AdvancedMessages
    ```

2. Using NuGet Packages Manager:

    To install the project using the NuGet Packages Manager, perform the following steps:

    1. Open your preferred Integrated Development Environment (IDE) that supports NuGet package management (e.g., Visual Studio).
    2. Create a new project or open an existing one.
    3. Select "Project" > "Manage NuGet Packages"
    4. In the "Browse" tab, search for the project package you want to install.
    5. Click on the "Install" button to add the selected package to your project.
    5. Follow any additional setup instructions or configurations provided in the project's documentation.

3. Downloading Source Code and Direct Linking:

    To install the project by downloading the source code and directly linking it to your project, adhere to the following steps:

    1. Visit the project repository on [GitHub](https://github.com/your-username/your-repo)
    2. Click on the "Code" button and select "Download ZIP" to download the project's source code as a zip archive.
    3. Extract the downloaded zip archive to the desired location on your local machine.
    4. Open your existing project or create a new one in your IDE.
    5. Add the downloaded project files to your solution using the "Add Existing Project" option in your IDE's solution explorer.
    6. Reference the project in your solution and ensure any required dependencies are resolved.
    7. Follow any additional setup or configuration instructions provided in the project's documentation.

Please note that each method may have specific requirements or configurations that need to be followed for successful installation.
Refer to the project's documentation for any additional steps or considerations.

**Do not forget to download and install appropriate localization pack from GitHub.**

## Usage

1. Update Delivery System

    ```C#
    BotBuilder.NewBuilder()
        .CustomDelivery(new AdvancedDeliverySystem())
        .Build()
        .Listen();
    ```

2. Custom messages and menus

    ```C#
    var mes = new MultiblockMessage();
    mes.AddBlock("Block section #1");
    mes.AddBlock("Block section #2");
    mes.AddMenu(new ReplyMenu(IS.Inputs.Anon_DemoTrain));
    ```

## Contributors

Currently, there are no contributors actively involved in this project.
However, our team is eager to welcome contributions from anyone interested in advancing the project's development.

We value every contribution and look forward to collaborating with individuals who share our vision and passion for this endeavor.
Your participation will be greatly appreciated in moving the project forward.

Thank you for considering contributing to our project.

## License

This project is distributed under the terms of the MIT License.

Copyright (C) Sargeras02 2023

## Developer contact

For any issues related to the project, please feel free to reach out to us through the project's GitHub page.
We welcome bug reports, feedback, and any other inquiries that can help us improve the project.

You can also contact the project owner directly via their GitHub profile at the following [link](https://github.com/Sargeras02).

Your collaboration and support are highly appreciated, and we will do our best to address any concerns or questions promptly and professionally.
Thank you for your interest in our project.

## Notes

Thank you for choosing our solution for your needs, and we look forward to contributing to your project's success.
