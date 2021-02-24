namespace Models.ViewModels
{
    public class ContactVm
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public bool IsContactNumberShowing { get; set; }
        public bool IsContactEmailShowing { get; set; }
    }
}
