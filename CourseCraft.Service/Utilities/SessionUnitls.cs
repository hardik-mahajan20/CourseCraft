
using CourseCraft.Repository.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace CourseCraft.Service.Utilities
{
    public static class SessionUtils
    {
        /// <summary>
        /// Stores user data (email and username) in the session.
        /// </summary>
        /// <param name="httpContext">The HTTP context to store the session data in.</param>
        /// <param name="user">The user object containing email and username.</param>
        public static void SetUser(HttpContext httpContext, UserLoginViewModel user)
        {
            if (user != null)
            {
                string userData = JsonSerializer.Serialize(user);
                httpContext.Session.SetString("UserData", userData);
            }
        }

        /// <summary>
        /// Clears all session data.
        /// </summary>
        /// <param name="httpContext">The HTTP context to clear the session data from.</param>
        public static void ClearSession(HttpContext httpContext) => httpContext.Session.Clear();
    }
}