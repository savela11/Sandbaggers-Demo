namespace Models.ViewModels
{
  public class BranchInfoVM
  {
    public int Id { get; set; }
    public string BranchId { get; set; }
    public bool IsActive { get; set; } = false;

    public string Street { get; set; } = "";
  }
}