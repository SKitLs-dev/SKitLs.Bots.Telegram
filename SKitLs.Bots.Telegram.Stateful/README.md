# SKitLs.Bots.Telegram.Stateful ![Static Badge](https://img.shields.io/badge/Follow%20GitHub%20-%20black?logo=github&link=https%3A%2F%2Fgithub.com%2FSargeras02%2FSKitLs.Bots.Telegram.git) ![GitHub](https://img.shields.io/github/license/Sargeras02/SKitLs.Bots.Telegram) ![Nuget](https://img.shields.io/nuget/v/SKitLs.Bots.Telegram.Stateful) [![CodeFactor](https://www.codefactor.io/repository/github/sargeras02/skitls.bots.telegram/badge)](https://www.codefactor.io/repository/github/sargeras02/skitls.bots.telegram)

An extension project built upon the SKitLs.Bots.Telegram.Core Framework.

Enables the segregation of user states.

## Description

SKitLs.Bots.Telegram.Stateful is a library that enables the segregation of user states and
facilitates the transformation of application logic.
It provides interfaces and components for handling updates, managing user states, and declaring state sections
for partitioning handling logic based on user states.

1. `interface IStatefulActionManager<TUpdate>` (default: `class StatefulActionManager<TUpdate>`):

    This interface serves as a central manager for handling updates and interactions via `IStateSection<TUpdate>` and `IBotAction` implementations.
    The manager acts as an orchestrator, storing and delegating incoming updates to the appropriate registered actions.
    It offers a simple architecture with a single storage collection for actions, providing a seamless integration with the
    existing `IActionManager<TUpdate>` and other related interfaces.

    Being the fourth architecture level, this interface serves as an add-on to the existing functionality.

2. `interface IStateSection<TUpdate>` (default: `DefaultStateSection<TUpdate>`):

    This interface offers the necessary mechanics for declaring and defining specific state sections.
    These sections serve as partitions for handling logic, allowing developers to segment interactions based on the user's state.
    By using state sections, you can organize and manage the bot's behavior more effectively, ensuring clarity and reusability of logic.
    The interface is equipped with methods for debugging, holding actions, and handling actions,
    making it a vital component in creating well-structured and maintainable bot code.

3. `interface IUserState` (default: `DefaultUserState`):

    The `IUserState` interface is responsible for providing the mechanics of user states.
    It allows for the declaration of custom user states and supports default user states through the DefaultUserState class.
    It ensures that user states are equatable and comparable based on integer values, enabling efficient state management and handling.

Overall, SKitLs.Bots.Telegram.Stateful empowers developers with the tools and interfaces to efficiently manage user states and
optimize the handling of interactions in Telegram bots.
The library's flexibility allows for customization and seamless integration with existing bot architectures,
making it an invaluable asset for enhancing the user experience and overall functionality of Telegram bots.

## Setup

### Requirements

Before running the project, please ensure that you have the following dependencies installed and properly configured in your development environment.

- Telegram.Bot 19.0.0 or higher
- SKitLs.Bots.Telegram.Core 2.3.0 or higher

### Installation

1. Using Terminal Command:
    
    To install the project using the terminal command, follow these steps:

    1. Open the terminal or command prompt.
    2. Run command:
    
    ```
    dotnet add package SKitLs.Bots.Telegram.Stateful
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

    1. Visit the project repository on [GitHub](https://github.com/SKitLs-dev/SKitLs.Bots.Telegram.git)
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

### Define user state:

```C#
IUserState state1 = new DefaultUserState(0, "Default");
IUserState state2 = new DefaultUserState(1, "Typing");
```

### Define state section and enable state:

```C#
IStateSection<SignedMessageTextUpdate> section = new DefaultStateSection<SignedMessageTextUpdate>();
// Enabled for any states by default
// OR Enable your state(s)
section.EnableState(state1);
section.EnableState(state2);

section.AddSafely(// some action);
```

### Use Stateful Manager:

```C#
IStatefulActionManager<SignedMessageTextUpdate> textInputManager = new StatefulActionManager<SignedMessageTextUpdate>();

// Add section
textInputManager.AddSectionSafely(section);

// Or if you have IApplicant applicant
// applicant.ApplyFor(textInputManager);

// Add it to handler
var privateText = new DefaultSignedMessageTextUpdateHandler();
privateText.TextInputManager = textInputManager;
```

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
