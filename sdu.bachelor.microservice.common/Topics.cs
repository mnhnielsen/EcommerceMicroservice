using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdu.bachelor.microservice.common
{
    public class Topics
    {
        public const string On_Basket_Checkout = "On_Checkout";
        public const string On_Order_Added_To_Basket = "On_Order_Added_To_Basket";
        public const string On_Order_Cancel = "On_Order_Cancel";
        public const string On_Product_Reserved_Failed = "On_Product_Reserved_Failed";
        public const string On_Product_Reserved = "On_Product_Reserved";
        public const string On_Product_Removed_From_Basket = "On_Product_Removed_From_Basket";

    }
}
