# SKitLs.Bots.Telegram. Sumup
**Status: work in progress.**

# An idea behind
Since I had discovered Telgram.Bot libarary I have reduced the time necessary for bot creation in times. But also I have faced the problem that my code becomes really complex and mostly contains of if-else constructs and repeating checkers for update's nullable content.

At that very moment I decided to review, collect and unify my fragmented code to build one generic solution for the most spreaded problems I have faced.
This library allows you not to waste your time for copy-pasting all the checkers and creating an architecture for your bot, but focus on designing its interior.

# Introduction
This project is a collection of libraries developed in C# with the aim of simplifying the process of creating bots for interacting with the Telegram messenger using the Telegram API. The main emphasis is placed on facilitating code writing and improving its readability.

By itself, the project does not provide any interaction with Telegram through its API and does not directly handle API requests.
The project depends on the _**Telegram.Bot**_ library, which is used to work with the Telegram API and provides the basic functionality for processing server messages.

The primary contribution of this project lies in the functionality of typing nullable fields, allowing end-users of the library to create class objects representing specific actions, rather than relying on numerous if-else constructs. The internal components of the project handle all necessary checks and data processing, streamlining bot development and simplifying program logic.

Ultimately, this project provides developers using C# with tools to create Telegram bots more quickly and efficiently. It reduces the amount of code required and enhances program readability by adopting an object-oriented approach and introducing field typing.

# General information
Commonly, each project contains _*.Prototype_ and _*.Model_ namespaces. First one contains interfaces and general solutions. Second one contains default realization of necessary interfaces.

Due to the fact my work is still in progress, not every project contains description. So, here some tips:
1. If you are intersted in quick setup, you can look through _*.Model_ namespace of a project.
2. If you are intersted in extending and overriding functional, pay attention to _*.Prototype_ namespace.

# Setup
Each project supports localized debugging. Some of them requires specific language packages. You can find them in `locals` folder.

After you have installed NuGet package, add `resources/locals` folders to your project and download appropriate language pack to this folder.

`resources/locals` path is a default one, you can update locals path and/or debug language with a following code.
```C#
BotBuilder.DebugSettings.DebugLanguage = LangKey.EN;
BotBuilder.DebugSettings.UpdateLocalsPath("resources/locals");
```

# SKitLs.Bots.Telegram.Core (Core)
**Status: Released**

An essential core module that contains main architecture and logic that handles and casts incoming updates. All the basics can be used and implemented in other modules to extend basic functinality.

This project is available as a NuGet package. To install it run:
```
dotnet add package SKitLs.Bots.Telegram.Core
```

# Quick Start
Create a new Console Project and paste this syntax directly into your Program class:
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
From now and on your bot is ready for use.
