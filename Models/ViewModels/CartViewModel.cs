using Models.EF;
using System.Collections.Generic;

namespace Models.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<CartItemsViewModel> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }

    public class CartRemoveViewModel
    {
        public decimal CartTotal { get; set; }
        public IEnumerable<CartItemsViewModel> CartItems { get; set; }
        public int ItemCount { get; set; }
        public bool Status { get; set; }
    }

    public class CartStatusToHeaderViewModel
    {
        public int ItemCount { get; set; }
        public IEnumerable<CartItemsViewModel> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }

    public class CompleteOrderViewModel
    {
        public IEnumerable<OrderDetail> OrderItems { get; set; }
        public bool ShipGender { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public decimal OrderTotal { get; set; }
    }

    public class CartItemsViewModel
    {
        public int RecordId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public string Image { get;set; }
        public string Name { get; set; }
        public string MetaTitle { get; set; }
        public decimal Price { get; set;}
    }
}