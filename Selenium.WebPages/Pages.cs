using GophotowebAT.AdminSite.Selenium.WebPages;
using GophotowebAT.CustomerSite.Selenium.WebPages;

namespace Selenium.WebPages
{
    public static class Pages
    {        
        public static readonly Homepage Homepage = new Homepage();
        public static readonly Clientarea Clientarea = new Clientarea();
        public static readonly ClientSites ClientSites = new ClientSites();
        public static readonly Createclientsite Createclientsite = new Createclientsite();
        public static readonly Clientsiteedit Clientsiteedit = new Clientsiteedit();

        public static readonly ProductPage ProductPage = new ProductPage();
        public static readonly Shop Shop = new Shop();
        public static readonly Cart Cart = new Cart();
        public static readonly OrderResultPage OrderResultPage = new OrderResultPage();

    }
}
