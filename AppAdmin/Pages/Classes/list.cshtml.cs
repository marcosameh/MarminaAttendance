﻿using App.Core.Entities;
using App.Core.Managers;
using App.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Classes
{
    [Authorize]

    public class listModel : PageModel
    {
        private readonly ClassManager classManager;
        private readonly TimeManager timeManager;

        public List<ClassVM> Classes { get; set; }
        [BindProperty]
        public App.Core.Entities.Classes classData { get; set; }
        public List<Time> TimeList { get; private set; }

        public listModel(ClassManager classManager, TimeManager timeManager)
        {
            this.classManager = classManager;
            this.timeManager = timeManager;
        }
        public void OnGet()
        {
            FillData();
        }
        public IActionResult OnGetDisplayClasses()
        {
            Classes = classManager.GetClasses();

            return new JsonResult(Classes);
        }
        public void OnPost()
        {
            var Result = classManager.AddClass(classData);

            TempData["NotificationType"] = Result.IsSuccess ? "success" : "error";
            TempData["Message"] = Result.IsSuccess ? "تم اضافة الفصل بنجاح" : Result.Error;
            FillData();

        }
        private void FillData()
        {
            TimeList = timeManager.GetTimeList();
        }
    }
}