namespace Blessings.Common.AspNet.Models
{
    public class ValidationMessageViewModel
    {
        public ValidationMessageViewModel()
        {
            Errors = new List<ValidationMessageViewModel>();
        }

        public int Status { get; set; }
        public string Message { get; set; }
        public List<ValidationMessageViewModel> Errors { get; set; }
       
    }
}
