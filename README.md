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
## About solution
Commonly, each project contains _*.Prototype_ and _*.Model_ namespaces. First one contains interfaces and general solutions. Second one contains default realization of necessary interfaces.

Due to the fact my work is still in progress, not every project contains description. So, here some tips:
1. If you are intersted in quick setup, you can look through _*.Model_ namespace of a project.
2. If you are intersted in extending and overriding functional, pay attention to _*.Prototype_ namespace.

## About this document
As I have tried to describe all things about solution and projects, this README became really complex and hard-to-read. To enhace your reading experience, I have added some markers to highlight some information.
1. **\[Dive in\]** - paragraphs noted with this marker contains information that could help you to dive in the project in case you are interested in creating more powerful on the solution's base.
2. **\[Focus\]** - paragraphs noted with this marker contains essential information. This information is recommended for reading.

If you are only interested in quick setup, check **"Quick Start"** and ![**"Setup, usage and examples"**] sections.

# SKitLs.Bots.Telegram.Core
An essential core module that contains main architecture and logic that handles and casts incoming updates. All the basics can be used and implemented in other modules to extend basic functinality.

## Features
The idea of this module is to unify incoming telegram updates. Three main aspects of this module are: casted updates, handling architecture and services.

### Casted Updates
```C#
namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
```

All the casted updates are implemented from `ICastedUpdate` interface that describes main information about an update.

For updates, that have a certain sender, an `ISignedUpdate : ICastedUpdate` interface is declared. 

At the moment only Message and Callback update classes are realized. Anyway any casted update can be realized directly in your project in the next way:

> ```C#
> public class SignedCallbackUpdate : CastedUpdate, ISignedUpdate
> {
>      // Your properties go here...
> }
> ```

For more information check *"Custom Updates"* section in *"Applications"*.

### **\[Dive in\]** Handling architecture
```C#
namespace SKitLs.Bots.Telegram.Core.Model;
namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
namespace SKitLs.Bots.Telegram.Core.Model.Management;
namespace SKitLs.Bots.Telegram.Core.Model.Interactions;
```

Overall there are five-level funnel of processing server updates.
1. **Step one: Bot Manager.**

   First step of this funnel is `BotManager`, which is a start point of all project. It contains general information and designed to link Telegram.Bots and SKitLs.Bots.Telegram libraries.

   BotManager contains some services, necessary for the work such as: Localiztor for getting localized strings (see: *"Applications"* > *"Localization"*) or Delievery System for converting SKitLs messages to API ones via declared interfaces and other.

   Furthermore, simple IoC-container is realized in the BotManager (private `Services` property). Container only supports Singletons, but can be accessed from any part of your code (see: *"Applications"* > *"Bot's Services"*) due to Owners System (see *"Features"* > *"Owners"*).

   After BotManager handled an update it is sent to one of ChatScanners.

2. **Step two: Chat Scanning.**

   Meanwhile BotManager is used to process global logic, Chat Scanners work only with certain updates (depends on thier `Telegram.Bot.[...].Update.ChatType` property). It helps to seperate logic into parts and save code clear.

   Each ChatType needs its own ChatScanner. But one ChatScanner can be subscribed for several ChatTypes.

   Chat Scanner consists of several Update Handlers. Each Update Handler `IUpdateHandlerBase<TUpdate>` only works with its own `TUpdate : ICastedUpdate`. 
  
3. **Step three: Handling Updates.**

   Update Handlers are realized via some default classes (ex: `DefaultCallbackHandler`), derived from `IUpdateHandlerBase<TUpdate>` where `TUpate` is a type of handling update (ex: `DefaultCallbackHandler : IUpdateHandlerBase<SignedCallbackUpdate>`).
   
   To sum up, each Update Handler is able to process updates of a certain `TUpdate` type and only from certain chats, depending on `ChatScanner` settings.

   > Note: Callback and Message updates are realized by default, but other Update Handlers looks like
   >```C#
   > public class ChatScanner
   > {
   >       public IUpdateHandlerBase<CastedUpdate>? ChatJoinRequestHandler { get; set; }
   > }
   >```
   > which means that you should be more patient, assigning these handlers and overriding handlers interior.
   > 
   > _This thing would be updated in future versions by adding all possible strongly typed casted updates._

   Update Handlers are final step of a global 'casting and handling' logic. At this moment updates are finally casted to specified `ICatedUpdate` types such as `SignedCallbackUpdate` and prepared to be passed to next steps.

---

Overall this three steps can be presented as a fork or a tree, where step by step raw server update is being unpacked and simplified. In that form they are ready to raise a certain action, declared by end-user.

![Bots Telegram Architecture](https://github.com/Sargeras02/SKitLs.Bots.Telegram/assets/38318884/6ae2ae35-50bc-44e1-92d3-c12817a2d976)

Though previous parts were focused on preparing incoming data for the work, fourth and fifth parts were created to make the process of programming more like an art rather than a routine. They include: Actions and Action Managers.

---

4. **Step four: Management.**

   Since an update was finally prepared in a certain Update Handler it could be sent to an Action Manager.

   Action Managers can be added to your custom Update Handler class. Default Update Handlers contains `IActionManager<TUpdate>` properties (`IActionManager<TUpdate> where TUpdate : ICastedUpdate`).
   
   ```C#
   public class DefaultSignedMessageTextUpdateHandler : IUpdateHandlerBase<SignedMessageTextUpdate>
   {
         public IActionManager<SignedMessageTextUpdate> CommandsManager { get; set; }
         public IActionManager<SignedMessageTextUpdate> TextInputManager { get; set; }
   }
   ```
   
   `IActionManager<TUpdate>` can be realized via `DefaultActionManager<TUpdate> : ILinearActionManager<TUpdate> where TUpdate : ICastedUpdate` by default.

   Default Action Managers are designed as **a collection of Actions** (see: _"Step Five"_), which are able to work with update of a type `TUpdate`. Manager can check an update and pass it to one or several valid Actions (validated by specified Action's checkers).
   
   ```C#
   public async Task ManageUpdateAsync(TUpdate update)
   {
      foreach (IBotAction<TUpdate> callback in Actions)
         if (callback.ShouldBeExecutedOn(update))
         {
            await callback.Action(update);
            // break;
         }
   }
   ```
   
   > **For example:** manager contains Сommand-Actions _'/start'_, _'/reset'_ and _'/rebuild'_. An incoming update is a Message Text with _**'/re'**_ content.
   > 
   > If Action's checker is 'Equality' (ex. `update.Text == action.ActionBase`) then none of Actions will be executed.
   >
   > But in case Action's checker is 'StartsWith' (ex. `action.ActionBase.StartsWith(update.Text)`), _'/**re**set'_ and _'/**re**build'_ commands will be executed.
  
5. **Final step: Actions.**

   Actions are used to make two things together: an action that should be **executed** and a rule **when** it should be to. This principle is illustrated in the previous example.

   All actions are derived from `IBotAction<TUpdate> : IBotAction where TUpdate : ICastedUpdate`, where `TUpdate` is an update this action should react on.

   > **For example:** callback action should react only on callback updates. So its realization is:
   > 
   > `DefaultCallback : DefaultBotAction<SignedCallbackUpdate>`

   Some defaults are `DefaultCallback` or `DefaultCommand`. But across the solution you can find such actions as
   ```C#
   public class BotArgCommand<TArg> : DefaultCommand, IArgedAction<TArg, SignedMessageTextUpdate> where TArg : notnull, new() { }
   public abstract class DefaultProcessBase : IBotProcess, IStatefulIntegratable<SignedMessageTextUpdate>, IBotAction<SignedMessageTextUpdate> { }
   ```
   Though they contain more complex logic, Action Managers are still able to handle them properly.

---

Probably, this funnel looks a bit scary and complex, but if are not intersted in diving into library interior you are still able to use prewritten defaults in your project and only code Actions logic to launch your bot. Their functionality covers all the basic needs.

You will find all the necessary information in _"Setup, usage and examples"_ paragraph.

### **\[Dive in\]** Owning
```C#
namespace SKitLs.Bots.Telegram.Core.Model.Building;
```
Though in 99 of 100 cases your project will have the only one object of a type `BotManager` (untill you are nesting several bots in one solution), BotManager is not created as a static one class to keep solution flexible and generic.

So how can you access Bot Manager's interior during runtime process? For these needs 'Owners System' is realized.

But BotManager's costructor is an internal one and a new object can be only created with a `BotBuilder` after all classes and services that need their Owner are already initialized. It means that instead of
```C#
BotBuilder.NewBuilder(token)
   .EnablePrivates(privates)
   .Build("Bot name")
   .Listen();
```
you will have to assign your .Build(...) object to some variable and then step-by-step reassign each Owner property, what is quite messy.

To prevent it, Dynamic Compilation is realized. Just implemet `IOwnerCompilable` interface to your class, that should be owned.

You can do it in the next way:
```C#
public class YourService : IOwnerCompilable
{
   private BotManager? _owner;
   public BotManager Owner
   {
      get => _owner ?? throw new NullOwnerException(GetType());
      set => _owner = value;
   }
   public Action<object, BotManager>? OnCompilation => null;
}
```
**How does it work?**

When `BotManager BotBuilder.Build()` is summoned, `BotManager.ReflectiveCompile()` is raised:
```C#
internal void ReflectiveCompile()
{
   GetType()
      .GetProperties()
      .Where(x => x.GetCustomAttribute<OwnerCompileIgnoreAttribute>() is null)
      .Where(x => x.PropertyType.GetInterfaces().Contains(typeof(IOwnerCompilable)))
      .ToList()
      .ForEach(refCompile =>
      {
         var cmpVal = refCompile.GetValue(this);
         if (cmpVal is IOwnerCompilable oc)
            oc.ReflectiveCompile(cmpVal, this);
      });

   Services.Values.Where(x => x is IOwnerCompilable)
      .ToList()
      .ForEach(service => (service as IOwnerCompilable)!.ReflectiveCompile(service, this));
}
```
and then in `IOwnerCompilable`:
```C#
public BotManager Owner { get; set; }
public Action<object, BotManager>? OnCompilation { get; }
public void ReflectiveCompile(object sender, BotManager owner)
{
   Owner = owner;
   OnCompilation?.Invoke(sender, owner);
   sender.GetType().GetProperties()
      .Where(x => x.GetCustomAttribute<OwnerCompileIgnoreAttribute>() is null)
      .Where(x => x.PropertyType.GetInterfaces().Contains(typeof(IOwnerCompilable)))
      .ToList()
      .ForEach(refCompile =>
      {
         var cmpVal = refCompile.GetValue(sender);
         if (cmpVal is IOwnerCompilable oc)
            oc.ReflectiveCompile(cmpVal, owner);
      });
}
```
all the necessary work would be done automatically.

To prevent your `IOwnerCompilable` from automatic assigning for some reason, you can use `OwnerCompileIgnoreAttribute`:
```C#
[OwnerCompileIgnore]
public ILocalizator Localizator => ResolveService<ILocalizator>();
```

## Setup and usage
This project is available as a NuGet package. To install it run:
```
dotnet add package SKitLs.Bots.Telegram.Core --version 1.4.1
```

To use project's facilities use `BotBuilder` and `ChatDesigner` classes. See [Functions and examples](#focus-functions-and-examples) for more info.

## **\[Focus\]** Functions and examples
BotManager - the heart of your bot - does not have public constructor. Use BotBuilder wizard constructor class instead.
1. Just raise static `BotBuilder.NewBuilder()` function to create a new Builder and explore its functinality.
2. Via BotBuilder you can design your BotManager for your needs, using closed, safe functions.
3. After you have set up all interior you can get your constructed BotManager with a `BotBuilder.Build()` function.
4. To activate your bot use `BotManager.Listen()` function.

The same process in appliable for ChatScanners, used for processing updates from chats, via their wizard constructor class `ChatDesigner`.

### Quick Start example
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

# SKitLs.Bots.Telegram.PageNavs

# SKitLs.Bots.Telegram.Stateful

# TODO: Applications
## Custom Updates (About creating your own custom update)
## Bot's Services
## Localization
