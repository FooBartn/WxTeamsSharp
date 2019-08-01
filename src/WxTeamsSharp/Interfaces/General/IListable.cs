namespace WxTeamsSharp.Interfaces.General
{
    /// <summary>
    /// Includes ListErrors
    /// </summary>
    public interface IListable
    {
        /// <summary>
        /// Potential error when get multiple results
        /// </summary>
        IListError Error { get; }
    }
}
