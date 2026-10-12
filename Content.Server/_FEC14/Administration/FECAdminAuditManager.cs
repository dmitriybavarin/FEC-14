using Content.Server.Administration.Logs;
using Content.Server.Chat.Managers;
using Content.Server.Discord;
using Content.Shared._FEC14.CCVar;
using Content.Shared.Database;
using Robust.Shared.Configuration;

namespace Content.Server._FEC14.Administration;

public sealed class FECAdminAuditManager
{
    [Dependency] private readonly IAdminLogManager _adminLog = default!;
    [Dependency] private readonly IChatManager _chat = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly DiscordWebhook _discord = default!;
    [Dependency] private readonly ILogManager _log = default!;

    public void Report(string message, LogImpact impact = LogImpact.High)
    {
        _adminLog.Add(LogType.AdminCommands, impact, $"{message}");
        _chat.SendAdminAlert(message);
        _log.GetSawmill("fec.admin.audit").Info(message);

        var url = _cfg.GetCVar(FECCVars.AdminAuditWebhook);
        if (string.IsNullOrWhiteSpace(url))
            return;

        _discord.TryGetWebhook(url, data => SendWebhook(data, message));
    }

    private async void SendWebhook(WebhookData data, string message)
    {
        try
        {
            await _discord.CreateMessage(data.ToIdentifier(), new WebhookPayload { Content = message });
        }
        catch (Exception e)
        {
            _log.GetSawmill("fec.admin.audit").Error($"Failed to send admin audit webhook: {e.Message}");
        }
    }
}
