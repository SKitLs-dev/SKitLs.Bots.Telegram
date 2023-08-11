# SKitLs.Bots.Telegram.PageNavs ![Static Badge](https://img.shields.io/badge/Follow%20GitHub%20-%20black?logo=github&link=https%3A%2F%2Fgithub.com%2FSargeras02%2FSKitLs.Bots.Telegram.git) ![GitHub](https://img.shields.io/github/license/Sargeras02/SKitLs.Bots.Telegram) ![Nuget](https://img.shields.io/nuget/v/SKitLs.Bots.Telegram.PageNavs) [![CodeFactor](https://www.codefactor.io/repository/github/sargeras02/skitls.bots.telegram/badge)](https://www.codefactor.io/repository/github/sargeras02/skitls.bots.telegram)

An extension project built upon the SKitLs.Bots.Telegram.Core Framework.

Allows to create special navigational menus.

## Description

SKitLs.Bots.Telegram.PageNavs library offers a set of tools to create custom menus with inline keyboards and callbacks.

1. `interface IMenuManager` (default: `class DefaultMenuManager`):

    The IMenuManager interface serves as a manager and a service for inline message navigation via callbacks and `IBotPage` pages.
    It collects, stores, and provides access to preset menu pages.
    This interface is an add-on at the architecture level and can be accessed via `BotManager.ResolveService<T>`.

2. `interface IBotPage` (defaults: `class WidgetPage`, `class StaticPage`):

    Used for creating special messages that can act as menu pages.

3. `interface IPageNavMenu` (default: `class PageNavMenu`):

    Provides methods for creating page menus with integrated navigation functionality.
    Streamlines the process of creating navigation menus for different pages within the bot.

## Setup

### Requirements

- SKitLs.Bots.Telegram.Core 2.0.0 or higher
- SKitLs.Bots.Telegram.ArgedInteraction 1.3.0 or higher
- SKitLs.Bots.Telegram.AdvancedMessages 1.1.0 or higher

Before running the project, please ensure that you have the following dependencies installed and properly configured in your development environment.

### Installation

1. Using Terminal Command:
    
    To install the project using the terminal command, follow these steps:

    1. Open the terminal or command prompt.
    2. Run command:
    
    ```
    dotnet add package SKitLs.Bots.Telegram.PageNavs
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

1. Define pages:

    ```C#
    // private IOutputMessage MainFormatter(ISignedUpdate? update) { //... }
    var mainBody = new DynamicMessage(MainFormatter);
    var mainMenu = new PageNavMenu();
    var mainPage = new WidgetPage("mainId", "Main", mainBody, mainMenu);

    // private IOutputMessage TodayFormatter(ISignedUpdate? update) { //... }
    var todayScheduleBody = new DynamicMessage(TodayFormatter);
    var todayScheduleMenu = new PageNavMenu();
    var todaySchedulePage = new WidgetPage("todayId", "Schedule (Today)", todayScheduleBody, todayScheduleMenu);
            
    // private IOutputMessage TomorrowFormatter(ISignedUpdate? update) { //... }
    var tomorrowScheduleBody = new DynamicMessage(TomorrowFormatter);
    var tomorrowSchedulePage = new WidgetPage("tomorrowId", "Schedule (Tomorrow)", tomorrowScheduleBody);

    // weekSchedulePage, upcomingSchedulePage
    ```

2. Set up and link menus:

    ```C#
    todayScheduleMenu.PathTo(tomorrowSchedulePage, weekSchedulePage, upcomingSchedulePage);

    // private IBotAction<SignedCallbackUpdate> DoDomeAction // ...
    mainMenu.PathTo(todaySchedulePage);
    mainMenu.AddAction(DoDomeAction);
    ```

3. Defines Menu Manager:

    ```C#
    var _mm = new DefaultMenuManager();

    _mm.Define(mainPage);
    _mm.Define(todaySchedulePage);
    _mm.Define(tomorrowSchedulePage);
    _mm.Define(weekSchedulePage);
    _mm.Define(upcomingSchedulePage);
    ```

4. **Do not forget to apply your manager to your Stateful Callback Manager:**

    ```C#
    _mm.ApplyTo(statefulCallbacks);
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
