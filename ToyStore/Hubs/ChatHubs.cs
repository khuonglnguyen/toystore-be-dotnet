﻿using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;
using ToyStore.Models;

namespace ToyStore.Hubs
{
    public class ChatHubs : Hub
    {
        public ChatHubs()
        {
            var tableDependency = new SqlTableDependency<Message>(ConfigurationManager.ConnectionStrings["ToyStore2021ConnectionString"].ConnectionString, tableName: "Message", schemaName: "dbo", executeUserPermissionCheck: false, includeOldValues: true);
            tableDependency.OnChanged += TableDependency_Changed;
            tableDependency.OnError += TableDependency_OnError;

            tableDependency.Start();
        }

        private void TableDependency_Changed(object sender, RecordChangedEventArgs<Message> e)
        {
            Show();
            ShowMessage();
        }

        private void TableDependency_OnError(object sender, ErrorEventArgs e)
        {

        }

        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHubs>();
            context.Clients.All.displayMessage();
        }

        public static void ShowMessage()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHubs>();
            context.Clients.All.displayMessageChating();
        }
    }
}