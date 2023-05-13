using App.Core.Entities;
using App.Core.Enums;
using App.Core.Managers;
using App.Core.Models;
using App.UI.Ifraustrcuture;
using AppCore.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Core.Types;
using OfficeOpenXml;
using System.Globalization;

namespace App.UI.Pages.Classes
{
    [Authorize(Roles = "SuperAdmin")]
    public class uploadModel : PageModel
    {
        private readonly ClassManager classManager;
        private readonly ServedManager servedManager;

        public SelectList ClassesSelectList { get; private set; }
        [BindProperty(SupportsGet = true)]
        public int ClassId { get; set; }

        [BindProperty(SupportsGet =true)]

        public IFormFile ExcelFile { get; set; }    
        public uploadModel(ClassManager classManager,ServedManager servedManager)
        {
            this.classManager = classManager;
            this.servedManager = servedManager;
        }

        public void OnGet()
        {
            FillData();
        }
        public async void OnPost()
        {
            var Result =await servedManager.ServedBulkInsertAsync(ExcelFile, ClassId);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم تحديث البيانات بنجاح" : Result.Error;
            FillData();

        }


        public void FillData()
        {
            var Classes = classManager.GetClasses();
            ClassesSelectList = new SelectList(Classes, "Id", "Name");
        }


       
    }

}
