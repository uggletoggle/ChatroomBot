﻿namespace ChatroomBot.Robot
{
    public class CurrentRecordValueMessage : GenericEvent
    {
        public CurrentRecordValueMessage()
        {
            Event = "CurrentRecordValue";
        }
        public string Symbol { get; set; }
        public double Close { get; set; }
    }
}
