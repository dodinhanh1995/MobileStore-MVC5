using System.Collections.Generic;

namespace Models.ViewModels
{
    public class AdvertisementViewModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Target { get; set; }
    }

    public class BannerViewModel
    {
        public IEnumerable<AdvertisementViewModel> SlideShows { get; set; }
        public IEnumerable<AdvertisementViewModel> HomeBanners { get; set; }
        public IEnumerable<AdvertisementViewModel> HomeAsides { get; set; }
    }
}
