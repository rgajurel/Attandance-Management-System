using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class Settings
    {
        public int Id { get; set; }
        [DisplayName("Identifier")]
        public string SettingsId { get; set; }
        [DisplayName("Key")]
        public string SettingsKey { get; set; }
        [DisplayName("Value")]
        public string SettingsValue { get; set; }
        [DisplayName("Default")]
        public string DefaultValue { get; set; }
        [DisplayName("Type")]
        public string Type { get; set; }
        public string Options { get; set; }
        public string OfGroup { get; set; }
        public int? SortOrder { get; set; }
        public string Status { get; set; }
        [DisplayName("Required")]
        public bool Required { get; set; }
        public bool ReadOnly { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string Regex { get; set; }
    }
}
