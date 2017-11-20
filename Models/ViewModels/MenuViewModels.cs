namespace Models.ViewModels
{
    public class MenuViewModels
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
        public int? DisplayOrder { get; set; }
        public string Target { get; set; }
        public bool? Status { get; set; }
        public int? TypeId { get; set; }
        public string TypeName { get; set; }
        public string Icon { get; set; }
    }

    public class NavigationViewModel
    {
        public string Text { get; set; }
        public string Link { get; set; }
        public string Target { get; set; }
        public string Icon { get; set; }
    }
}
