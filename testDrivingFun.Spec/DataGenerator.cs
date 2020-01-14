using System;
using TestDrivingFun.Console;
using TestDrivingFun.Engine;

namespace testDrivingFun.Spec
{
    internal static class DataGenerator
    {
        public static Message TestMessage()
        {
            return new Message("id", "causationid", "correlationId", DateTime.Now);
        }
    }
}