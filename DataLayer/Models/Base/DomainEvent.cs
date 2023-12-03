using DataLayer.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Base
{
    public abstract class DomainEvent
    {
        public string ExchangeName { get; set; }

        public string[] Routes { get; set; } = Array.Empty<string>();

        public EventType EventType { get; set; } = EventType.Add;
        public Dictionary<string, object> Payload { get; set; } = new Dictionary<string, object>();

        public DomainEvent()
        {
            ExchangeName = "STATE_CHANGES";
            Routes = new[] { "Pezeshkafzar" };
        }

    }
}
