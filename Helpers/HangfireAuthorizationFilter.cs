using Hangfire.Dashboard;

namespace Car_Dealership_System.Helpers
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // Allow local requests only or for any authenticated user in production adjust as needed
            return true;
        }
    }
}