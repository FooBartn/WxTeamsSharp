namespace WxTeamsSharp.Interfaces.People
{
    /// <summary>
    /// PersonBuilder Interface
    /// </summary>
    public interface IBuildablePerson
    {
        /// <summary>
        /// Will build into a createable person. I.e. can run CreateAsync()
        /// </summary>
        /// <returns></returns>
        ICreateablePerson Build();
    }
}
