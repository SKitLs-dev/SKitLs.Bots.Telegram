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

# SKitLs.Bots.Telegram.Core
An essential core module that contains main architecture and logic that handles and casts incoming updates. All the basics can be used and implemented in other modules to extend basic functinality.

## Features
The idea of this module is to unify incoming telegram updates. Three main aspects of this module are: casted updates, handling architecture and services.
### Casted Updates
**> `SKitLs.Bots.Telegram.Core.Model.UpdatesCasting`**
All the casted updates are implemented from `ICastedUpdate` interface that describes main information about an update.

For updates, that have a certain sender, an `ISignedUpdate : ICastedUpdate` interface is declared. 

At the moment only Message and Callback update classes are realized. Anyway any casted update can be realized directly in your project in the next way:

> `public class SignedCallbackUpdate : CastedUpdate, ISignedUpdate
> {
>   // Your properties go here...
> }`

For more information check *"Custom Updates"* section in *"Applications"*.

### **\[Dive in\]** Handling architecture
**> SKitLs.Bots.Telegram.Core.Model**

**> SKitLs.Bots.Telegram.Core.Model.UpdateHandlers**

**> SKitLs.Bots.Telegram.Core.Model.Management**

**> SKitLs.Bots.Telegram.Core.Model.Interactions**

Overall there are five-level funnel of processing server updates.
1. **First step of this funnel is `BotManager` (`*.Model`).**

   BotManager is a start point of all project. It contains general information and designed to link Telegram.Bots and SKitLs.Bots.Telegram libraries.

   BotManager contains some services, necessary for the work such as: Localiztor for getting localized strings (see: *"Applications"* > *"Localization"*) or Delievery System for converting SKitLs messages to API ones via declared interfaces and other.

   Furthermore, simple IoC-container is realized in the BotManager (private `Services` property). Container only supports Singletons, but can be accessed from any part of your code (see: *"Applications"* > *"Bot's Services"*) due to Owners System (see *"Features"* > *"Owners"*).

   After BotManager handled an update it is sent to one of ChatScanners

2. **Second step: Chat Scanning (`*.Model`).**

   Meanwhile BotManager is used to process global logic, Chat Scanners work only with certain updates (depends on thier `Telegram..Bot.[...].Update.ChatType` property). It helps to seperate logic into parts and save code clear.

   Each Chat Type needs its own Chat Scanner. But one Chat Scanner can be subscribed for several Chat Types.

   Chat Scanner consists of several Update Handlers. Each Update Handler `IUpdateHandlerBase<TUpdate>` only works with its own `TUpdate : ICastedUpdate`. 

   To sum up, each Update Handler is able to process updates of a certain `TUpdate` type and only from a certain chats, depending on `ChatScanner` settings.

   > Note: Callback and Message updates are realized by default, but other Update Handlers looks like
   >
   > `public class ChatScanner
   > {
   >    public IUpdateHandlerBase<CastedUpdate>? ChatJoinRequestHandler { get; set; }
   > }`
   >
   > which means that you should be more patient, assigning these handlers and overriding handlers interior.
   > 
   > _This thing would be updated in future versions by adding all possible strongly typed casted updates._
  
4. **Step three: Handling Updates (`*.Model.UpdateHandlers`).**

   Update Handlers are realized via some default classes (ex `DefaultCallbackHandler`), derived from `IUpdateHandlerBase<TUpdate>` where `TUpate` is a type of handling update (ex: `DefaultCallbackHandler : IUpdateHandlerBase<SignedCallbackUpdate>`).

   Update Handlers are final step of a global 'casting and handling' process. At this moment updates are finally casted to specified `ICatedUpdate` types such as `SignedCallbackUpdate` and prepared to be passed to next steps.

---

Overall this three steps can be presented as a fork or a tree, where step by step raw server update is being unpacked and simplified. In that form they are ready to raise a certain action, declared by end-user.

![Bots Telegram Architecture](https://github.com/Sargeras02/SKitLs.Bots.Telegram/assets/38318884/6ae2ae35-50bc-44e1-92d3-c12817a2d976)

Though previous parts were focused on preparing incoming data for the work, fourth and fifth parts were created to make the process of programming more like an art rather than a routine. They include: Actions and Action Managers.

4. **Step four: Management (`*.Model.Management`)**

   Since an update was finally prepared in a certain Update Handler it could be sent to an Action Manager.

   Action Managers can be added to your custom Update Handler class. Default Update Handlers contains `IActionManager<TUpdate>` properties (`public interface IActionManager<TUpdate> where TUpdate : ICastedUpdate`).
   `IActionManager<TUpdate>` can be realized via `public class DefaultActionManager<TUpdate> : ILinearActionManager<TUpdate> where TUpdate : ICastedUpdate` by default.

   Default Action Managers are designed as _a collection of Actions_ (see: _"Step Five"_), which are able to work with update of a type `TUpdate`. Manager can check an update and pass it to one or several valid Actions (validated by specified Action's checkers).

   > **For example:** manager contains command Actions _'/start'_, _'/reset'_ and _'/rebuild'_. An incoming update is a Message Text with _**'/re'**_ content.
   > 
   > If Action's checker is 'Equality' (ex. `update.Text == action.ActionBase`) then none of Actions will be executed.
   >
   > But in case Action's checker is 'StartsWith' (ex. `action.ActionBase.StartsWith(update.Text)`), _'/**re**set'_ and _'/**re**build'_ commands will be executed.
  
5. **Final step: Actions (`*.Model.Interactions`)**

   Actions are used to make two things together: an action that should be **executed** and a rule **when** it should be to. This principle is illustrated in the previous example.

   All actions are derived from `public interface IBotAction<TUpdate> : IBotAction where TUpdate : ICastedUpdate`, where `TUpdate` is an update this action should react on.

   > **For example:** callback action should react only on callback updates. So its realization is:
   > 
   > `DefaultCallback : DefaultBotAction<SignedCallbackUpdate>`

   Some defaults are `DefaultCallback` or `DefaultCommand`. But across the solution you can find such actions as `BotArgCommand<TArg> : DefaultCommand, IArgedAction<TArg, SignedMessageTextUpdate> where TArg : notnull, new()` or `DefaultProcessBase : IBotProcess, IStatefulIntegratable<SignedMessageTextUpdate>, IBotAction<SignedMessageTextUpdate>`. Though they contain more complex logic, Action Managers are still able to handle them properly.

Probably, this funnel looks a bit scary and complex, but if are not intersted in diving into library interior you are still able to use prewritten defaults in your project and only code Actions logic to launch your bot. Their functionality covers all the basic needs.

You will find all the necessary information in _"Setup, usage and examples"_ paragraph.

### Owners
Though in 99 of 100 cases BotManager will have the only one object 

## Setup, usage and examples.

## Functions and examples

# SKitLs.Bots.Telegram.PageNavs

# SKitLs.Bots.Telegram.Stateful

# Applications
## Custom Updates (About creating your own custom update)
## Bot's Services
## Localization
