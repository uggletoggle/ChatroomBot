using ChatroomBot.API.Entities;
using ChatroomBot.API.Hubs;
using ChatroomBot.API.Models;
using Microsoft.AspNetCore.SignalR;

using System;
using System.Text.Json;

namespace ChatroomBot.API.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public EventProcessor(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public void ProcessEvent(string message)
        {
            var eventType = GetEventType(message);
            Message messageEntity;

            switch (eventType)
            {
                case EventType.CurrentStockValue:
                    var messageObj = JsonSerializer.Deserialize<CurrentRecordValueMessage>(message);
                    var messageTextFormatted = ConstructMessageFromEvent(messageObj);

                    messageEntity = new Message
                    {
                        Text = messageTextFormatted,
                        User = "StockBot",
                        Timestamp = DateTime.UtcNow
                    };

                    _hubContext.Clients.All.SendAsync("ReceiveMessage", messageEntity);
                    ;break;

                case EventType.NotFoundValue:
                    var messageObjNotFound = JsonSerializer.Deserialize<NotFoundValueMessage>(message);

                    messageEntity = new Message
                    {
                        Text = $"Not found current value for {messageObjNotFound.Symbol}. Check format and spelling and try again.",
                        User = "StockBot",
                        Timestamp = DateTime.UtcNow
                    };

                    _hubContext.Clients.All.SendAsync("ReceiveMessage", messageEntity);
                    ; break;

                case EventType.RobotWorkerError:
                    var messageObjRobotError = JsonSerializer.Deserialize<RobotWorkerErrorMessage>(message);

                    messageEntity = new Message
                    {
                        Text = $"ERROR --> {messageObjRobotError.ExceptionType} - {messageObjRobotError.Description}",
                        User = "StockBot",
                        Timestamp = DateTime.UtcNow
                    };

                    _hubContext.Clients.All.SendAsync("ReceiveMessage", messageEntity);
                    ; break;

                default:
                    Console.WriteLine("--> Event Type not supported")
                    ;break;
            }
        }

        private string ConstructMessageFromEvent(CurrentRecordValueMessage message) => $"{message.Symbol} quote is ${message.Close} per share";

        private EventType GetEventType(string message)
        {
            var eventType = JsonSerializer.Deserialize<GenericEvent>(message);

            switch (eventType.Event)
            {
                case "CurrentRecordValue":
                    return EventType.CurrentStockValue;
                case "NotFoundValue":
                    return EventType.NotFoundValue;
                case "RobotWorkerError":
                    return EventType.RobotWorkerError;
                default:
                    return EventType.Undetermined;
            }
        }
    }
    enum EventType
    {
        CurrentStockValue,
        NotFoundValue,
        RobotWorkerError,
        Undetermined
    }
}
