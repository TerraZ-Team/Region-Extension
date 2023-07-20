﻿using System;
using TShockAPI;

namespace RegionExtension.Commands.Parameters
{
    public interface ICommandParam
    {
        string Name { get; }
        string Description { get; }
        object Value { get; }
        bool Optional { get; }
        bool Dynamic { get; }
        int Count { get; }
        public string GetBracketName();
        public string GetColoredBracketName();
        bool TrySetValue(string str, CommandArgsExtension args = null);
        bool TrySetDefaultValue(CommandArgsExtension args = null);
        bool TrySetDynamicValue(CommandArgsExtension args = null);
        public ICommandParam[] GetAdditionalParams();
    }
}