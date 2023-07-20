# SKitLs.Bots.Telegram. Sumup
**Status: work in progress.**

# An idea behind
Since I had discovered Telgram.Bot libarary I have reduced the time necessary for bot creation in times. But also I have faced the problem that my code becomes really complex and mostly contains of if-else constructions and repeating checkers for update's nullable content.
At that very moment I decided to review, collect and unify my fragmented code to build one generic solution for the most spreaded problems I faced.
This library allows you not to waste your time for copy-pasting all the checkers and creating an architecture for your bot, but focus on designing its interior.

# Introduction
This project is a collection of libraries developed in C# with the aim of simplifying the process of creating bots for interacting with the Telegram messenger using the Telegram API. The main emphasis is placed on facilitating code writing and improving its readability.

By itself, the project does not provide any interaction with Telegram through its API and does not directly handle API requests.
The project depends on the Telegram.Bot library, which is used to work with the Telegram API and provides the basic functionality for processing server messages.

The primary contribution of this project lies in the functionality of typing nullable fields, allowing end-users of the library to create class objects representing specific actions, rather than relying on numerous if-else constructs. The internal components of the project handle all necessary checks and data processing, streamlining bot development and simplifying program logic.

Ultimately, this project provides developers using C# with tools to create Telegram bots more quickly and efficiently. It reduces the amount of code required and enhances program readability by adopting an object-oriented approach and introducing field typing.

# SKitLs.Bots.Telegram.Core
An essential core module that contains main architecture and logic. Handles and casts incoming updates. All the basics can be used and implemented in other modules to extend basic functinality.

## Features
The idea of this module is to unify incoming telegram updates. Three main aspects of this module are: casted updates, handling architecture and services.
### Casted Updates
**> `SKitLs.Bots.Telegram.Core.Model.UpdatesCasting`**
All the casted updates are implemented from `ICastedUpdate` interface that describes main information about an update. For updates that has certain sender an `ISignedUpdate : ICastedUpdate` interface is added. 

At the moment only Message and Callback update classes are realized.
Anyway any casted update can be realized directly in your project in the next way:

> `public class SignedCallbackUpdate : CastedUpdate, ISignedUpdate
> {
>   // Your properties go here...
> }`

For more information check "Custom Updates" section in "Applications"

### Handling architecture
**> SKitLs.Bots.Telegram.Core.Model**

**> SKitLs.Bots.Telegram.Core.Model.UpdateHandlers**

**> SKitLs.Bots.Telegram.Core.Model.Management**

**> SKitLs.Bots.Telegram.Core.Model.Interactions**

Overall there are five-level funnel of processing server updates.
1. **First step of this funnel is `BotManager` (`*.Model`).**

   BotManager is main handler. It contains general information and designed as a bridge between Telegram.Bots and SKitLs.Bots.Telegram projects.

   BotManager contains some services necessary for the work such as: Localiztor for localization or Delievery System (used for converting SKitLs messages to proper library ones) via proper interface.

   Furthermore, simple IoC-container is realized in the BotManager (private `Services` property). Container only supports Singletons. For more information see "Applications" > "Bot's Services".

   After BotManager hadled an update it is sent to one of ChatScanners

2. **Second step: Chat Scanning (`*.Model`).**

   Meanwhile BotManager is used to handle global logic, Chat Scanners handle only certain updates (by thier `Update.ChatType`) in a proper way. Each Chat Type needs its own Chat Scanner. But one Chat Scanner can be subscribed for several chat types.

   Chat Scanner only handles updates from the chats, which type matchs their settings. It helps to seperate logic into parts and save code clear.

   Chat Scanner contains several Update Handlers. Each handler handles only its own typed updates and since each Chat Scanner contains its own Handlers - it's possible to write that each handlers handles updates only with a certain type and only from a certain chats.

   > Note: Callback and Message updates are realized by default, but other Handlers looks like
   > `public IUpdateHandlerBase<CastedUpdate>? ChatJoinRequestHandler { get; set; }`
   > which means that you should be more patient, handling this updates.
   > It would be updated in future versions.
  
3. **Step three: Handling Updates (`*.Model.UpdateHandlers`).**

   Update Handlers are realized via Default Update Handlers classes (ex `DefaultCallbackHandler`), derived from `IUpdateHandlerBase<TUpdate>` where `TUpate` is a type of handling update (ex: `DefaultCallbackHandler : IUpdateHandlerBase<SignedCallbackUpdate>`).

   Update Handlers are final step of a global handling and casting process. At the this moment updates are finally casted to specified casted type such as `SignedCallbackUpdate`.

Overall this three steps can be imagined as a fork or a tree, where step by step raw server update is being unwrapped and simplified to get a certain resulting class.

Though previos parts were focused on preparing incoming data for the work, fourth and fifth parts were created to make the process of programming more like an art rather than a routine. They include: Action and Action Managers

4. **Step four: Management (`*.Model.Management`)**

   Since an update was finally prepared in a certain Update Handler it could be sent to an Action Manager.

   Action Managers can be added to your custom Update Handler class. Default Update Handlers contains `IActionManager<TUpdate>` properties (`public interface IActionManager<TUpdate> : IDebugNamed, IOwnerCompilable, IActionsHolder where TUpdate : ICastedUpdate`).
   `IActionManager<TUpdate>` can be realized via `public class DefaultActionManager<TUpdate> : ILinearActionManager<TUpdate> where TUpdate : ICastedUpdate` by default.

   Default Action Managers are designed as a collection of Actions (see: Step Five), which are able to work with update of a type `TUpdate`, that can check an update and pass it to valid Actions (validated by specified Action's checkers).

   > **For example:** manager contains command Actions '/start', '/reset' and '/rebuild'. An incoming update is Message Text one with '/re' content.
   > If Action's checker is equality (`update.Text == action.ActionBase`) then none of Actions will be executed.
   > But in case Action's checker is kinda '.StartsWith' (`action.ActionBase.StartsWith(update.Text)`), '/reset' and '/rebuild' commands will be executed.
  
5. **Final step: Actions (`*.Model.Interactions`)**

   Actions are used to make two things together: an action that should be **executed** and a rule **when** it should be. This principle is illustrated in the previos example.

   All actions are derived from `public interface IBotAction<TUpdate> : IBotAction, IEquatable<IBotAction<TUpdate>> where TUpdate : ICastedUpdate`, where `TUpdate` is an update this action should react on.

   Some defaulties are `DefaultCallback : DefaultBotAction<SignedCallbackUpdate>` or `DefaultCommand : DefaultBotAction<SignedMessageTextUpdate>`. But across the solution you can face such actions as `public class BotArgCommand<TArg> : DefaultCommand, IArgedAction<TArg, SignedMessageTextUpdate> where TArg : notnull, new()` or `public abstract class DefaultProcessBase : IBotProcess, IStatefulIntegratable<SignedMessageTextUpdate>, IBotAction<SignedMessageTextUpdate>`.

   Action Managers are still will be able to handle that ugly and scary actions properly. 

## Setup, usage and examples.

## Functions and examples

# SKitLs.Bots.Telegram.PageNavs

# SKitLs.Bots.Telegram.Stateful

# Applications
## Custom Updates (About creating your own custom update)
## Bot's Services
