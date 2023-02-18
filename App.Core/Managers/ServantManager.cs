using App.Core.Entities;
using SharedKernel.Core.Common;

namespace App.Core.Managers
{
    public class ServantManager
    {
        private readonly MarminaAttendanceContext context;

        public ServantManager(MarminaAttendanceContext context)
        {
            this.context = context;
        }
        public Result AddServant(Servants newServant)
        {
            try
            {
                context.Servants.Add(newServant);   
                context.SaveChanges();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

        }


    }
}
