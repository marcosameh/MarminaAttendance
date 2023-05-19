using App.Core.Managers;
using AppCore.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.UI.Pages.Serveds
{
    [Authorize(Roles = "SuperAdmin")]
    public class uploadModel : PageModel
    {
        private readonly ClassManager classManager;
        private readonly ServedManager servedManager;

        public SelectList ClassesSelectList { get; private set; }
        [BindProperty(SupportsGet = true)]
        public int ClassId { get; set; }

        [BindProperty(SupportsGet = true)]

        public IFormFile ExcelFile { get; set; }
        public uploadModel(ClassManager classManager, ServedManager servedManager)
        {
            this.classManager = classManager;
            this.servedManager = servedManager;
        }

        public void OnGet()
        {
            FillData();
        }
        public async Task<IActionResult> OnPost()
        {
            FillData();
            var Result = await servedManager.ServedBulkInsertAsync(ExcelFile, ClassId);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم تحديث البيانات بنجاح" : Result.Error;
            if (Result.IsFailure)
            {
                return Page();
            }
            return Redirect($"/class/edit/{ClassId}");
        }


        public void FillData()
        {
            var Classes = classManager.GetClasses();
            ClassesSelectList = new SelectList(Classes, "Id", "Name");
        }



    }

}
