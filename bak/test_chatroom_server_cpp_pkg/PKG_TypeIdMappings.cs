#pragma warning disable 0169, 0414
using TemplateLibrary;

[TypeIdMappings]
interface ITypeIdMappings
{
    Chat.Room _1 { get; }

    Chat.User _2 { get; }

    Chat.Message _13 { get; }

    Chat.MessageEx _14 { get; }

    Client_To_ChatServer.Login _3 { get; }

    Client_To_ChatServer.Enter _4 { get; }

    Client_To_ChatServer.Send _5 { get; }

    ChatServer_To_Client.Login.Success _6 { get; }

    ChatServer_To_Client.Login.Fail _7 { get; }

    ChatServer_To_Client.Enter.Success _8 { get; }

    ChatServer_To_Client.Enter.Fail _9 { get; }

    ChatServer_To_Client.Send.Fail _10 { get; }

    ChatServer_To_Client.Send.Success _11 { get; }

    ChatServer_To_Client.Broadcast.Message _12 { get; }

}
