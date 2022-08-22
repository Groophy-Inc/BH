namespace BH.Structes.ErrorStack
{
    public class Error
    {
        public ErrorPathCodes ErrorPathCode { get; set; }
        public int ErrorID { get; set; }
        public int DevCode { get; set; }
        public string ErrorMessage { get; set; }
        public int HighLightLen { get; set; }
        public string line { get; set; }
    }

    public class DetailedError
    {
        public ErrorPathCodes ErrorPathCode { get; set; }
        public int ErrorID { get; set; }
        public int DevCode { get; set; }
        public string ErrorMessage { get; set; }
        public int HighLightLen { get; set; }
        public string line { get; set; }
        public string ErrPath { get; set; }
        public int TotalIndexOfLineWords { get; set; }
        public int LineC { get; set; }
        public string Date { get; set; }
    }
        
    public enum ErrorPathCodes
    {
        Program = 0,
        Parser = 1,
        ErrorHandle = 2,
        Script = 3,
        Builder = 4,
        APF = 5,
        Server = 6,
        Structes = 7,
        Varriables = 8
    }
}