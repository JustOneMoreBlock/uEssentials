/*
 *  This file is part of uEssentials project.
 *      https://uessentials.github.io/
 *
 *  Copyright (C) 2015-2016  Leonardosc
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License along
 *  with this program; if not, write to the Free Software Foundation, Inc.,
 *  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/

using Essentials.Api.Command.Source;
using Essentials.Common;
using Essentials.Common.Util;
using Essentials.Core;

namespace Essentials.Api.Command
{
    public abstract class EssCommand : ICommand
    {
        protected string        UsageMessage    => "Use /" + Name + " " + Usage;
        public string           Name            { get; internal set; }
        public string           Permission      { get; set; }
        public string[]         Aliases         { get; set; }
        public string           Usage           { get; set; }
        public AllowedSource    AllowedSource   { get; set; }
        public string           Description     { get; set; }

        protected EssCommand()
        {
            var commandInfo = Preconditions.NotNull(
                ReflectionUtil.GetAttributeFrom<CommandInfo>( this ),
                "EssCommand must have 'CommandInfo' attribute" );

            Name = commandInfo.Name;
            Usage = commandInfo.Usage;
            Description = commandInfo.Description;
            AllowedSource = commandInfo.AllowedSource;
            Aliases = commandInfo.Aliases;

            Permission = GetType().Assembly.Equals( typeof (EssCore).Assembly )
                ? $"essentials.command.{Name}"
                : commandInfo.Permission;

            if ( Permission.IsNullOrEmpty() )
                Permission = Name;
        }

        protected virtual void ShowUsage( ICommandSource source )
        {
            source.SendMessage( UsageMessage );
        }

        protected virtual void OnUnregistered()
        {
        }

        protected virtual void OnRegistered()
        {
        }

        public abstract CommandResult OnExecute( ICommandSource src, ICommandArgs args );
    }
}