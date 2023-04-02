using Hangfire.Dashboard;

namespace App.UI.InfraStructure
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // Implement your authentication logic here
            return true; // Allow all authenticated users to access the Dashboard
        }
    }
}
