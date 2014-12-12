using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schols.Models
{
    public class ScholarshipLink
    {
        // for the scholarship searches, not all scholarship fields are returned so this class is for the minimal
        public string FUND_ACCT;
        public string SCHLRSHP_NUM;
        public string FRML_SCHLRSHP_NAME;
        public string fav;
    }
}