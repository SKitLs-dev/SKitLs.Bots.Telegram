# SKitLs.Bots.Telegram.ArgedInteractions ![Static Badge](https://img.shields.io/badge/Follow%20GitHub%20-%20black?logo=github&link=https%3A%2F%2Fgithub.com%2FSargeras02%2FSKitLs.Bots.Telegram.git) ![GitHub](https://img.shields.io/github/license/Sargeras02/SKitLs.Bots.Telegram) ![Nuget](https://img.shields.io/nuget/v/SKitLs.Bots.Telegram.ArgedInteractions) [![CodeFactor](https://www.codefactor.io/repository/github/sargeras02/skitls.bots.telegram/badge)](https://www.codefactor.io/repository/github/sargeras02/skitls.bots.telegram)

An extension project built upon the SKitLs.Bots.Telegram.Core Framework.

Provides a structured and efficient mechanism for the serialization and deserialization of textual data

Need a boost? Jump to [Usage](#usage) section!

## Description

SKitLs.Bots.Telegram.ArgedInteractions is an extension project built upon the SKitLs.Bots.Telegram.Core Framework.
Its primary objective is to provide a structured and efficient mechanism for the serialization and deserialization
of textual data to facilitate the creation of specialized actions that support arguments.

SKitLs.Bots.Telegram.ArgedInteractions is a project that enhances the capabilities of the SKitLs.Bots.Telegram.Core Framework
by introducing essential methods for handling text-based data.
The project focuses on organizing the serialization and deserialization processes, which are crucial for implementing actions
with argument support in Telegram bots.

Key Features:

1. Textual Data Serialization:

    The project offers robust methods to convert textual data into a serializable format.
    These methods ensure that the text-based information can be efficiently transmitted and utilized within the Telegram bot ecosystem.

2. Textual Data Deserialization:

    SKitLs.Bots.Telegram.ArgedInteractions includes advanced techniques for deserializing received text-based data.
    This allows the bot to interpret and extract relevant information from incoming messages and updates, making it possible to handle user input effectively.

3. Specialized Actions with Argument Support:

    The project's core focus lies in empowering developers to create specialized bot actions that can accept arguments.
    These actions are designed to process textual data with attached arguments, enabling the bot to execute complex and context-sensitive tasks.

4. Seamless Integration:

    SKitLs.Bots.Telegram.ArgedInteractions seamlessly integrates with the existing SKitLs.Bots.Telegram.Core Framework.
    The project harmoniously leverages the core functionality while providing an additional layer of support for argument-based interactions.

## Setup

### Requirements

- SKitLs.Bots.Telegram.Core 2.3.0 or higher
- Telegram.Bot 19.0.0 or higher

Before running the project, please ensure that you have the following dependencies installed and properly configured in your development environment.

### Installation

1. Using Terminal Command:
    
    To install the project using the terminal command, follow these steps:

    1. Open the terminal or command prompt.
    2. Run command:
    
    ```
    dotnet add package SKitLs.Bots.Telegram.ArgedInteractions
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

    1. Visit the project repository on [GitHub page](https://github.com/SKitLs-dev/SKitLs.Bots.Telegram.git).
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

### Registering Custom Serialization Rule

To serialize specific types of data in a Telegram bot, you can register custom serialization rules using the
`IArgsSerializeService` interface. This allows the bot to handle complex data structures effectively.
 
Here's an example of registering a custom serialization rule for a custom data type:

```C#
IArgsSerializeService argsSerializeService = new DefaultArgsSerializeService();
argsSerializeService.AddRule<MyCustomDataType>(input =>
{
    MyCustomDataType customDataInstance;
    // Custom logic to convert the string input into MyCustomDataType
    // ... (implementation details)
    return ConvertResult<MyCustomDataType>.Success(customDataInstance);
});
```

### Creating an Argumented Action (ex. Callback)

You can create specialized bot actions that support arguments using `IArgedAction<TArg, TUpdate>` classes
(`BotArgedCommand<TArg>`, `BotArgedCallback<TArg>`, `BotArgedTextInput<TArg>`).
This allows the bot to respond to user interactions with context-specific actions.

Below is an example of creating an argumented callback:

```C#
LabeledData labeledData = new LabeledData("This is label", "argedCallbackId");
BotArgedInteraction<MyCustomDataType, SignedCallbackUpdate> customArgAction = async (args, update) =>
{
    // Custom logic to handle the callback with the provided arguments
    // ... (implementation details)
};

// Creating the BotArgedCallback instance
BotArgedCallback<MyCustomDataType> argedCallback = new BotArgedCallback<MyCustomDataType>(labeledData, customArgAction);
```

And implement it into Telegram.Bot.InlineKeyboardMarkup with:

```C#
MyCustomDataType data = new(...);
var callbackData = argedCallback.GetSerializedData(data, argsSerializeService);
_ = InlineKeyboardMarkup.WithCallbackData(argedCallback.Label, callbackData);
```

_You can use [SKitLs.Bots.Telegram.AdvancedMessages](https://www.nuget.org/packages/SKitLs.Bots.Telegram.AdvancedMessages/) package to get better messaging experience._

## Contributors

Currently, there are no contributors actively involved in this project.
However, our team is eager to welcome contributions from anyone interested in advancing the project's development.

We value every contribution and look forward to collaborating with individuals who share our vision and passion for this endeavor.
Your participation will be greatly appreciated in moving the project forward.

Thank you for considering contributing to our project.

## License

This project is distributed under the terms of the MIT License.

Copyright (C) SKitLs 2023

## Developer contact

For any issues related to the project, please feel free to reach out to us through the project's GitHub page.
We welcome bug reports, feedback, and any other inquiries that can help us improve the project.

You can also contact the project owner directly via their GitHub profile at the [following link](https://github.com/SKitLs-dev) or email: skitlsdev@gmail.com

Your collaboration and support are highly appreciated, and we will do our best to address any concerns or questions promptly and professionally.
Thank you for your interest in our project.

## Notes

Thank you for choosing our solution for your needs, and we look forward to contributing to your project's success.
