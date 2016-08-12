using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using System.Web;
using EP.IdentityIsolation.Infra.CrossCutting.Identity.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace EP.IdentityIsolation.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationUserManager _userManager;

        public ChatHub(ApplicationUserManager userManager)
        {
            
            _userManager = userManager;
            
        }

        public override Task OnConnected()
        {
            AssignToSecurityGroup();
            Greet();

            return base.OnConnected();
        }

        private void AssignToSecurityGroup()
        {
            if (Context.User.Identity.IsAuthenticated)
                Groups.Add(Context.ConnectionId, "authenticated");
            else
                Groups.Add(Context.ConnectionId, "anonymous");
        }

        private void Greet()
        {
            var greetedUserName = Context.User.Identity.IsAuthenticated ?
                Context.User.Identity.Name :
                "anonymous";

            Clients.Client(Context.ConnectionId).OnMessage(
                "[server]", "Welcome to the chat room, " + greetedUserName);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            RemoveFromSecurityGroups();
            return base.OnDisconnected(stopCalled);
        }

        private void RemoveFromSecurityGroups()
        {
            Groups.Remove(Context.ConnectionId, "authenticated");
            Groups.Remove(Context.ConnectionId, "anonymous");
        }

        [Authorize]
        public void SendMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            BroadcastMessage(message);
        }

        private void BroadcastMessage(string message)
        {
            //var userName = _userManager.FindByEmail(Context.User.Identity.Name).UserName;
            var userName = Context.User.Identity.Name;

            Clients.All.OnMessage(userName, message);

            //Clients.Group("authenticated").OnMessage(userName, message);

            //var excerpt = message.Length <= 3 ? message : message.Substring(0, 3) + "...";
            //Clients.Group("anonymous").OnMessage("[someone]", excerpt);
        }

        protected override void Dispose(bool disposing)
        {
            //base.Dispose(disposing);
        }
    }
}
