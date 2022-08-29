using System;

public class ValidationMessageViewModel
{
    public int Status { get; set; }
    public string Message { get; set; }
    public List<ValidationMessageViewModel> Errors { get; set; }

    public ValidationMessageViewModel()
    {
        Errors = new List<ValidationMessageViewModel>();
    }
}
