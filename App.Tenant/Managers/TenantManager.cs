using App.Tenant.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace App.Tenant.Managers
{
    public class TenantManager
    {
        private readonly IHttpContextAccessor accessor;
        private readonly SharedTenantContext sharedtenantContext;

        public TenantManager(IHttpContextAccessor Accessor, SharedTenantContext sharedtenantContext)
        {
            accessor = Accessor;
            this.sharedtenantContext = sharedtenantContext;
        }
        public Entities.Tenant GetCurrentTenant()
        {
        
                var CurrentDomin = accessor.HttpContext.Request.Host.Value;

            return sharedtenantContext.Tenant.Where(x => x.Domain == CurrentDomin).FirstOrDefault();


        }
    }
}
