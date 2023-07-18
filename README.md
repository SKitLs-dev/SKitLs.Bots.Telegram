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

## Description

## Setup and usage

## Functions and examples

This is an essential, core module that contains all the basics which can be used and implemented in other modules to extend basic functinality.  

# SKitLs.Bots.Telegram.PageNavs

# SKitLs.Bots.Telegram.Stateful

Подскажи, мне, пожалуйста, следующее: как правильно составить описание для моего open-source проекта, который является набором инструментов для проектирования телеграмм-бота
