using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using WizardWebPage.Classes;

namespace WizardWebPage
{
    public class GhostTracker
    {
        private readonly static Lazy<GhostTracker> _instance = new Lazy<GhostTracker>(() => new GhostTracker(GlobalHost.ConnectionManager.GetHubContext<GhostHub>().Clients));

        private readonly ConcurrentDictionary<string, GhostPosition> positions = new ConcurrentDictionary<string, GhostPosition>();

        private  readonly  object _updatePositionsLock = new object();

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(1000);
        private readonly Timer _timer;

        private GhostTracker(IHubConnectionContext clients)
        {
            Clients = clients;

            _timer = new Timer(UpdatePositions, null, _updateInterval, _updateInterval);
        }

        public static GhostTracker Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext Clients
        {
            get;
            set;
        }


        public void AddOrUpdateGhost(GhostPosition ghost)
        {
            positions.AddOrUpdate(ghost.name, ghost, (s, position) => ghost);
        }

        public void RemoveGhost(string id)
        {
            GhostPosition position;
            positions.TryRemove(id, out position);
        }

        private void UpdatePositions(object state)
        {
            lock (_updatePositionsLock)
            {
                if (positions.Count == 0)
                    return;


                /*
                TODO: Ensure this works, then apply it. Maybe sure the Unity endpoint is expecting this as well
                List<GhostPosition> positionList = positions.Select(ghostPosition => ghostPosition.ConvertToString()).ToList();
                //TODO: Do this with string builder
                var payload = "";
                foreach (var ghostString in positionList)
                {
                    payload += ghostString;
                }
                Clients.All.updatePositions(payload);
                */

                List<GhostPosition> positionList = positions.Select(ghostPosition => ghostPosition.Value).ToList();
                var payload = JsonConvert.SerializeObject(positionList);
                Clients.All.updatePositions(payload);
            }
        }
    }
}
