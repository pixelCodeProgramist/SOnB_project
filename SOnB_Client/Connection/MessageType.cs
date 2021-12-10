using System;
using System.Text.RegularExpressions;

namespace SOnB_Client.Connection
{
    public enum MessageType
    {
        CommunicationError, CrcMessage, CommunicationClosed
    }

    public static class MessageTypeUtility
    {
        public static MessageType GetMessage(string message)
        {
            switch (message)
            {
                case "Connection error":
                    return MessageType.CommunicationError;
                case "":
                    return MessageType.CommunicationClosed;
                case var crc when new Regex(@"^[0-1]+$").IsMatch(crc):
                    return MessageType.CrcMessage;
                default:
                    throw new InvalidOperationException("Message cannot be handled");
            }
        }
    }
}
