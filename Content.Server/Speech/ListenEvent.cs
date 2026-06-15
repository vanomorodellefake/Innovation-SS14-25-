// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Speech;

public sealed class ListenEvent : EntityEventArgs
{
    public readonly string Message;
    public readonly string OriginalMessage; // IS-edit
    public readonly EntityUid Source;

    public ListenEvent(string message, string originalMessage, EntityUid source)
    {
        Message = message;
        OriginalMessage = originalMessage; // IS-edit
        Source = source;
    }
}

public sealed class ListenAttemptEvent : CancellableEntityEventArgs
{
    public readonly EntityUid Source;

    public ListenAttemptEvent(EntityUid source)
    {
        Source = source;
    }
}
