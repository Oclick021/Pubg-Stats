using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using PubgStatsBot.Helpers;

// Inherit from PreconditionAttribute
public class IsOwner : PreconditionAttribute
{

    // Override the CheckPermissions method
    public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        // If this command was executed by a user with the appropriate role, return a success
        if (context.User.Id == Credentials.UserID)
            // Since no async work is done, the result has to be wrapped with `Task.FromResult` to avoid compiler errors
            return Task.FromResult(PreconditionResult.FromSuccess());
        // Since it wasn't, fail
        else
            return Task.FromResult(PreconditionResult.FromError($"Only Owner can use this command"));

    }
}
