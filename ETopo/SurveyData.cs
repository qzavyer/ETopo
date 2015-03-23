using System;
using System.Collections.Generic;

namespace ETopo
{
    public class SurveyData
    {
        private DateTime _date { get; set; }

        public string Name { get; set; }

        public DateTime? Date
        {
            get
            {
                if (_date == DateTime.MinValue) return null;
                return _date;
            }
            set { _date = value == null ? new DateTime() : value.Value; }
        }

        public List<string> Team { get; set; }
    }
}
