using App.Core.Entities;
using App.Core.Managers;
using App.UI.Ifraustrcuture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Pages.Servant
{
    public class EditcshtmlModel : PageModel
    {
        private readonly ServantManager servantManager;
        private readonly WeekManager weekManager;
        private readonly int NumberOfWeeksAppearInMarkup = 16;
        [BindProperty(SupportsGet =true)]
        public int Id { get; set; }
        public List<Weeks> WeeksList { get; private set; }
        public Servants Servant { get; private set; }

        public EditcshtmlModel(ServantManager servantManager,WeekManager weekManager)
        {
            this.servantManager = servantManager;
            this.weekManager = weekManager;
        }
        public void OnGet()
        {
            FillData();
        }
        public void FillData()
        {
           
            WeeksList = weekManager.GetWeeks(NumberOfWeeksAppearInMarkup);
            Servant = servantManager.GetServant(Id);
        }
    }
}
