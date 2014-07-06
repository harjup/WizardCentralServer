using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using WizardWebPage.Classes;

namespace WizardWebPage
{
    public class GhostHub : Hub
    {
        private readonly GhostTracker _ghostTracker;

        public GhostHub() : this(GhostTracker.Instance) { }

        public GhostHub(GhostTracker ghostTracker)
        {
            _ghostTracker = ghostTracker;
        }

        public override Task OnConnected()
        {
            _ghostTracker.AddOrUpdateGhost(new GhostPosition(){ name = Context.ConnectionId, position = "(0,0,0)"});
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            _ghostTracker.RemoveGhost(Context.ConnectionId);
            return base.OnDisconnected();
        }

        //TODO: Hook up to client
        //When the player enters the hub they should add their ghost to the list
        public void ActivateGhost()
        {
            _ghostTracker.AddOrUpdateGhost(new GhostPosition() { name = Context.ConnectionId, position = "(0,0,0)" });
        }

        //TODO: Hook up to client
        // When the player leaves the hub their ghost shouldn't show up in other games
        // and they should stop receiving ghost broadcasts
        public void DeactivateGhost()
        {
            _ghostTracker.RemoveGhost(Context.ConnectionId);
        }

        public void SetGhostPosition(string transform)
        {
            _ghostTracker.AddOrUpdateGhost(new GhostPosition() { name = Context.ConnectionId, position = transform});
        }

        
    }
}