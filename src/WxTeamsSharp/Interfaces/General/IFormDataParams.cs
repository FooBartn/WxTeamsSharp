using System.Net.Http;

namespace WxTeamsSharp.Interfaces.General
{
    /// <summary>
    /// Can export to MultipartFormData
    /// </summary>
    public interface IFormDataParams
    {
        /// <summary>
        /// Export to MultipartFormData
        /// </summary>
        /// <returns></returns>
        MultipartFormDataContent ToFormData();
    }
}
