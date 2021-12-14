using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToyStore.ListenerModels
{
    public class ListenerOrder
    {
        public int ID { get; set; }

        public int CustomerID { get; set; }

        public System.DateTime DateOrder { get; set; }

        public System.DateTime DateShip { get; set; }

        public int Offer { get; set; }

        public bool IsPaid { get; set; }

        public bool IsCancel { get; set; }

        public bool IsDelete { get; set; }

        public bool IsDelivere { get; set; }

        public bool IsApproved { get; set; }

        public bool IsReceived { get; set; }

        public Nullable<decimal> Total { get; set; }
    }
}