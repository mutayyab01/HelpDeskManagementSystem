using HelpDeskSystem.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace HelpDeskSystem.AuditsManager
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }
        public EntityEntry Entry { get; set; }
        public string UserId { get; set; }
        public string TableName { get; set; }
        public string Module { get; set; }
        public string IpAddress { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new();
        public Dictionary<string, object> OldValues { get; } = new();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();

        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns { get; } = new();

        public AuditTrail ToAudit()
        {
            var auditTrail = new AuditTrail();
            auditTrail.UserId = UserId;
            auditTrail.Action = AuditType.ToString();
            auditTrail.AffectedTable = TableName;
            auditTrail.TimeStamp = DateTime.Now;
            auditTrail.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
            auditTrail.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            auditTrail.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            auditTrail.AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
            auditTrail.Module = Module;
            return auditTrail;
        }

    }
}
