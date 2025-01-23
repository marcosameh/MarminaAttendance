using App.Core.Entities;

namespace App.Core.Models
{
    public class BithdayEmailModel
    {
        public List<Servants> Servants { get; set; }
        public List<Served> BirthdayServed { get; set; }

        public BithdayEmailModel(List<Servants> servants, List<Served> serveds)
        {
            Servants = servants;
            BirthdayServed = serveds;
        }

    }
}
