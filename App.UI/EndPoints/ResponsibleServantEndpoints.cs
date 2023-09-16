using App.Core.Managers;

namespace MarminaAttendanceAPI.Endpoints
{
    public static class ResponsibleServantEndpoints
    {
        public static void ConfigureResponsibleServantEndpoints(this WebApplication app)
        {

            app.MapGet("/api/responsible-servant/{classId}", (int classId, ServantManager servantManager) =>
            {
                var responsibleServants = servantManager.GetResponsibleServants(classId);

                if (responsibleServants is null)
                {
                    return Results.BadRequest("No Servants In this Class"); // Return the error message
                }

                return Results.Ok(responsibleServants);
            });




        }



    }
}
