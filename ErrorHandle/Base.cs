using System;
using System.Collections.Generic;

namespace BH.ErrorHandle
{
    public enum BatchLogTypes
    {
        Error,
        Log
    }
    public class BatchLog
    {
        public BatchLogTypes type { get; set; }
        public DetailedError Typeof_Error { get; set; }
        public string Typeof_Log { get; set; }
    }
    internal class Base
    {

    }
}
