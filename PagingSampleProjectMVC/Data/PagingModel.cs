namespace PagingSampleProjectMVC.Data
{
    public class PagingModel
    {
        public int P { get; set; } = 1;
        public int S { get; set; } = 10;
        public int TotalRecords { get; set; } = 0;
    }
}
