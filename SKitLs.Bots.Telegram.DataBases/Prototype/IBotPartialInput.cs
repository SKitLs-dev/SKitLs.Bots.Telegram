﻿using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using System.Reflection;

namespace SKitLs.Bots.Telegram.DataBases.Prototype
{
    public interface IBotPartialInput
    {
        public Task<IOutputMessage> GetMesFor(PropertyInfo property, string terminationalKey);
    }
}