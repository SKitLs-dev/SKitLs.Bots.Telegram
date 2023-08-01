# SKitLs.Bots.Telegram.Core ![Static Badge](https://img.shields.io/badge/Follow%20GitHub%20-%20black?logo=github&link=https%3A%2F%2Fgithub.com%2FSargeras02%2FSKitLs.Bots.Telegram.git) ![GitHub](https://img.shields.io/github/license/Sargeras02/SKitLs.Bots.Telegram) ![Nuget](https://img.shields.io/nuget/v/SKitLs.Bots.Telegram.Core) [![CodeFactor](https://www.codefactor.io/repository/github/sargeras02/skitls.bots.telegram/badge)](https://www.codefactor.io/repository/github/sargeras02/skitls.bots.telegram)

The core module of the SKitLs.Bots.Telegram Framework.

## Description

SKitLs.Bots.Telegram.Core is a central package of the SKitLs.Bots.Telegram framework, providing essential mechanics for interfacing with the Telegram API.
Serving as a wrapper for the Telegram.Bot project, it acts as a core foundation for Telegram bot development.

At the heart of this package lies the `BotManager` class, designed to streamline the interaction process with server updates
and unpack them into strictly typed `ICastedUpdate` classes.
By leveraging the capabilities of BotManager, developers can automate the handling of updates received from the Telegram servers,
facilitating the efficient processing of incoming data.

Furthermore, BotManager incorporates a straightforward Inversion of Control (IoC) container,
ensuring seamless management and configuration of dependencies within the framework.

SKitLs.Bots.Telegram.Core empowers developers with a reliable and robust foundation to build Telegram bots with ease.
Its comprehensive features and well-structured design facilitate the development process, allowing developers
to focus on implementing bot logic without the hassle of handling low-level API interactions.

## Setup

### Requirements

- Telegram.Bot 19.0.0 or higher
- SKitLs.Utils.LocalLoggers 1.3.0 or higher

Before running the project, please ensure that you have the following dependencies installed and properly configured in your development environment.

### Installation

1. Using Terminal Command:
    
    To install the project using the terminal command, follow these steps:

    1. Open the terminal or command prompt.
    2. Run command:
    
    ```
    dotnet add package SKitLs.Bots.Telegram.Core
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

    1. Visit the project repository on [GitHub](https://github.com/Sargeras02/SKitLs.Bots.Telegram.git)
    2. Click on the "Code" button and select "Download ZIP" to download the project's source code as a zip archive.
    3. Extract the downloaded zip archive to the desired location on your local machine.
    4. Open your existing project or create a new one in your IDE.
    5. Add the downloaded project files to your solution using the "Add Existing Project" option in your IDE's solution explorer.
    6. Reference the project in your solution and ensure any required dependencies are resolved.
    7. Follow any additional setup or configuration instructions provided in the project's documentation.

Please note that each method may have specific requirements or configurations that need to be followed for successful installation.
Refer to the project's documentation for any additional steps or considerations.

## Usage

See [GitHub page](https://github.com/Sargeras02/SKitLs.Bots.Telegram.git) for more information.

### Setup

Each project supports localized debugging. Some of them requires specific language packages.
You can find them in [GitHub's](https://github.com/Sargeras02/SKitLs.Bots.Telegram.git) locals folder.

Place locals in "resources/locals" or update localization path.

```C#
BotBuilder.DebugSettings.DebugLanguage = LangKey.EN;
BotBuilder.DebugSettings.UpdateLocalsPath("path/to/data");
```

### Examples

1. Quick Start

    ```C#
    static async Task Main(string[] args)
    {
        var privateMessages = new DefaultSignedMessageUpdateHandler();
        var privateTexts = new DefaultSignedMessageTextUpdateHandler
        {
            CommandsManager = new DefaultActionManager<SignedMessageTextUpdate>()
        };
        privateTexts.CommandsManager.AddSafely(StartCommand);
        privateMessages.TextMessageUpdateHandler = privateTexts;
   
        ChatDesigner privates = ChatDesigner.NewDesigner()
            .UseMessageHandler(privateMessages);
   
        await BotBuilder.NewBuilder("YOUR_TOKEN")
            .EnablePrivates(privates)
            .Build()
            .Listen();
    }

    private static DefaultCommand StartCommand => new("start", Do_StartAsync);
    private static async Task Do_StartAsync(SignedMessageTextUpdate update)
    {
        await update.Owner.DeliveryService.ReplyToSender("Hello, world!", update);
    }
    ```

2. Add More Actions

    ```C#
    private static DefaultCommand StartCommand => new("start", Do_StartAsync);
    private static async Task Do_StartAsync(SignedMessageTextUpdate update)
    {
        await update.Owner.DeliveryService.ReplyToSender("Hello, world!", update);
    }
    
    private static DefaultCommand ExitInput => new("Exit bot!", Do_ExitAsync);
    private static DefaultCommand ExitCommand => new("exit", Do_ExitAsync);
    private static async Task Do_ExitAsync(SignedMessageTextUpdate update)
    {
        await update.Owner.DeliveryService.ReplyToSender("I'm Done!", update);
    }

    private static DefaultCallback HelloCallback => new("helloCB", Do_HelloAsync);
    private static async Task Do_HelloAsync(SignedCallbackUpdate update)
    {
        // Acces Telegram.Bot bot instance if necessary
        await update.Owner.Bot.EditMessageTextAsync(update.TriggerMessageId, "Updated Body!")
    }
    ```

    _You can use SKitLs.Bots.Telegram.AdvancedMessages package to get better messaging experience._

3. Use Services
    
    * Add service to bot

        ```C#
        await BotBuilder.NewBuilder("YOUR_TOKEN")
            .EnablePrivates(privates)
            .AddServices<ISomeService>(new SomeService())
            .Build()
            .Listen();
        ```

    * Resolve one when needed
        
        ```C#
        private static DefaultCommand StartCommand => new("start", Do_StartAsync);
        private static async Task Do_StartAsync(SignedMessageTextUpdate update)
        {
            var someService = update.Owner.ResolveService<ISomeService>();
            await update.Owner.DeliveryService.ReplyToSender(someService.GetContentFor(update.Text), update);
        }
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
