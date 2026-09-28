using System;
using Server.Accounting;
using Server.Logging;

namespace Server.Misc;

/// <summary>
/// Creates the owner account on an unattended first boot. With no accounts, UOContent's
/// AccountPrompt asks for one on the console, which throws when the server is headless.
/// This runs first and creates the owner from OWNER_USERNAME and OWNER_PASSWORD instead.
/// The credentials are read from the environment only, never written to configuration.
/// </summary>
public static class HeadlessOwner
{
    private static readonly ILogger logger = LogFactory.GetLogger(typeof(HeadlessOwner));

    // Ahead of AccountPrompt.Initialize (default priority 0).
    [CallPriority(-1)]
    public static void Initialize()
    {
        if (!Core.Headless || Accounts.Count > 0)
        {
            return;
        }

        var username = Environment.GetEnvironmentVariable("OWNER_USERNAME");
        var password = Environment.GetEnvironmentVariable("OWNER_PASSWORD");

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
        {
            throw new InvalidOperationException(
                "This headless server has no accounts. Set OWNER_USERNAME and OWNER_PASSWORD to create the owner account on first boot."
            );
        }

        var account = new Account(username.Trim(), password)
        {
            AccessLevel = AccessLevel.Owner
        };

        ServerAccess.AddProtectedAccount(account, true);
        logger.Information("Owner account created: {Username}", account.Username);
    }
}
