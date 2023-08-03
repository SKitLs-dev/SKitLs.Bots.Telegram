# SKitLs.Bots.Telegram.BotProcesses ![Static Badge](https://img.shields.io/badge/Follow%20GitHub%20-%20black?logo=github&link=https%3A%2F%2Fgithub.com%2FSargeras02%2FSKitLs.Bots.Telegram.git) ![GitHub](https://img.shields.io/github/license/Sargeras02/SKitLs.Bots.Telegram) ![Nuget](https://img.shields.io/nuget/v/SKitLs.Bots.Telegram.BotProcesses) [![CodeFactor](https://www.codefactor.io/repository/github/sargeras02/skitls.bots.telegram/badge)](https://www.codefactor.io/repository/github/sargeras02/skitls.bots.telegram)

An extension project built upon the SKitLs.Bots.Telegram.Core Framework.

Provides methods of creating bot processes with advanced textual input mechanics.

## Description

SKitLs.Bots.Telegram.BotProcesses is a powerful library designed to facilitate the implementation of a model that assigns personalized
buffers to individual users, each containing their current active tasks.
This library is targeted for use in Telegram bot development, enabling seamless integration with bot processes.

1. `interface IBotProcess` and `interface IBotRunningProcess`:

    The `IBotProcess` interface defines the contract for a bot process.
    Each process is assigned a unique identifier, and it maintains its state, which represents the user-specific state information
    associated with the process. The `IBotProcess` used for creation a descriptive model of a process, meanwhile `IBotRunningProcess`
    represents launched for a certain user process.

    The `IBotRunningProcess` interface extends the functionalities of the `IBotProcess` interface, providing additional methods to manage running bot processes.
    It includes methods to launch a process with updates and handle input during the process execution.

2. `interface IProcessManager` (default: `class DefaultProcessManager`):

    The IProcessManager interface provides functionalities for managing bot processes.
    It allows defining individual processes or collections of processes.
    The manager can initiate a bot process based on specific arguments and user updates.
    Additionally, it provides methods to retrieve running processes and terminate them.

3. Default Processes (via `TextInputsProcessBase<TArg>`):

    The `TextInputsProcessBase<TArg>` abstract class serves as the foundation for input processes.
    Derived from this class, default input processes can be created with custom process arguments `TArg`, which must implement the `IProcessArgument` interface.

    One of its key features lies in the provision of essential base solutions inherited from the `TextInputsProcessBase<TArg>` class,
    which significantly simplifies the implementation of various bot functionalities.
    
    * ComplexShotInputProcess
    * ShotInputProcess
    * IntInputProcess
    * PartialInputProcess
    
    By leveraging the base solutions provided by SKitLs.Bots.Telegram.BotProcesses, you can focus on implementing bot-specific functionalities,
    reducing development time, and ensuring a robust and interactive Telegram bot experience.

With SKitLs.Bots.Telegram.BotProcesses, you can build interactive Telegram bots and manage personalized tasks efficiently,
making it an essential addition to your Telegram bot development toolkit.


## Setup

### Requirements

- Telegram.Bot 19.0.0 or higher
- SKitLs.Bots.Telegram.Core 2.0.0 or higher
- SKitLs.Bots.Telegram.Stateful 1.1.0 or higher
- SKitLs.Bots.Telegram.AdvancedMessages 1.1.0 or higher
- SKitLs.Bots.Telegram.ArgedInteractions 1.3.1 or higher

Before running the project, please ensure that you have the following dependencies installed and properly configured in your development environment.

### Installation

1. Using Terminal Command:
    
    To install the project using the terminal command, follow these steps:

    1. Open the terminal or command prompt.
    2. Run command:
    
    ```
    dotnet add package SKitLs.Bots.Telegram.BotProcesses
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

**Do not forget to download and install appropriate localization pack from GitHub.**

## Usage

1. Use process manager:

    ```C#
    var _pm = new DefaultProcessManager();
    BotBuilder.NewBuilder("token")
        .AddService<IProcessManager>(_pm)
        .Build()
        .Listen();
    ```

2. Initialize processes:

    ```C#
    private static async Task When_ProcessCompletedAsync(TextInputsArguments<YourType> args, SignedMessageTextUpdate update)
    {
        if (args.CompleteStatus == CompleteStatus.Success)
        {
            await this.SendToDatabaseExample<YourType>(args.BuildingInstance);
        }
        // Some inform action
        await InformSenderAsync(args.CompleteStatus, update);
    }
    ```

    ```C#
    var procState =  DefaultUserStatenew(1001, "procId.state");
    var message = new OutputMessageText("Please, fill all details:");
    ShotInputProcess<YourType> process = new("procId", "Cancel", procState, message, When_ProcessCompletedAsync);
    ```

3. Define them:

    ```C#
    // Resolve if necessary
    // var _pm = _botManager.ResolveService<IProcessManager>();

    _pm.Define(process);
    ```

4. Access and run defined process anytime:

    ```C#
    priate async Task UpdateHandledAsync(SignedMessageTextUpdate update)
    {
        var _pm = update.Owner.ResolveService<IProcessManager>();
        await _pm.GetDefined("procId")
            .GetRunning(update.Sender.TelegramId, new TextInputsArguments(new YourType()))
            .LauchWith(update);
    }
    ```

5. Enhancing experience:

    **Do not forget** to define appropriate rules for converting. For this example:

    ```C#
    // Building logic
    var serializer = _botManager.ResolveService<IArgsSerializeService>();
    serializer.AddRule<YourType>(input =>
    {
        YourType customDataInstance;
            // Custom logic to convert the string input into YourType
            // ... (implementation details)
        return ConvertResult<YourType>.Success(customDataInstance);
    });
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
