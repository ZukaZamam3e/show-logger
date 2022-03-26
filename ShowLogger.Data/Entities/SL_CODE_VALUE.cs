namespace ShowLogger.Data.Entities;

public class SL_CODE_VALUE
{
    public int CODE_TABLE_ID { get; set; }

    public int CODE_VALUE_ID { get; set; }

    public string DECODE_TXT { get; set; }

    public string? EXTRA_INFO { get; set; }
}

public enum CodeTableIds
{
    SHOW_TYPE_ID = 1,
}

public enum CodeValueIds
{
    TV = 1000,
    MOVIE = 1001,
    AMC = 1002,
}
