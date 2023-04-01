using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Ifraustrcuture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.UI.Pages.Servant
{
    public class EditcshtmlModel : PageModel
    {
        private readonly ServantManager servantManager;
        private readonly WeekManager weekManager;
        private readonly ClassManager classManager;
        private readonly int NumberOfWeeksAppearInMarkup = 16;
        [BindProperty(SupportsGet =true)]
        public int Id { get; set; }
        public List<Weeks> WeeksList { get; private set; }
        [BindProperty]
        public Servants Servant { get; set; }
        [BindProperty]
        public ServantWeeksDTO ServantWeeksDTO { get; set; }
        public SelectList Classes { get; private set; }

        public EditcshtmlModel(ServantManager servantManager,WeekManager weekManager,ClassManager classManager)
        {
            this.servantManager = servantManager;
            this.weekManager = weekManager;
            this.classManager = classManager;
        }
        public void OnGet()
        {
            FillData();
        }
        public void FillData()
        {

            WeeksList = weekManager.GetWeeks(NumberOfWeeksAppearInMarkup);
            Servant = servantManager.GetServant(Id);
            Classes = new SelectList(classManager.GetClasses(), "Id", "Name");
        }
        public void OnPost()
        {
            if (Servant.PhotoFile != null)
            {
                Servant.Photo = FileManager.UploadPhoto(Servant.PhotoFile, "/wwwroot/photos/الخدام/", 285, 310);

            }
            var Result = servantManager.UpdateServant(Servant,ServantWeeksDTO);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم تحديث البيانات بنجاح" : Result.Error;
            FillData();
           
        }
    }
}
