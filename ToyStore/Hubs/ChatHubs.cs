using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace ToyStore.Hubs
{
    public class ChatHubs : Hub
    {
        public ChatHubs()
        {
            //var tableDependency = new SqlTableDependency<ListenerOrder>(ConfigurationManager.ConnectionStrings["ToyStore2021ConnectionString"].ConnectionString, tableName: "Order", schemaName: "dbo", executeUserPermissionCheck: false, includeOldValues: true);
            //tableDependency.OnChanged += TableDependency_Changed;
            //tableDependency.OnError += TableDependency_OnError;

            //tableDependency.Start();
        }

        //private void TableDependency_Changed(object sender, RecordChangedEventArgs<ListenerOrder> e)
        //{
        //    Show();
        //}

        //private void TableDependency_OnError(object sender, ErrorEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHubs>();
            context.Clients.All.displayMessage();
        }
    }
}