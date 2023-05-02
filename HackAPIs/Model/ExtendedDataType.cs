namespace HackAPIs.Model
{
    /// <summary>
    /// Data beyond the base to to get or update
    /// </summary>
    public enum ExtendedDataType
    {
        // None,       // 0: This never happens, removed
        BaseOnly,    // 1
        Solutions,   // 2
        Skills,     // 3
        UpdateADId, // 4
        GithubId    // 5
    }
}
