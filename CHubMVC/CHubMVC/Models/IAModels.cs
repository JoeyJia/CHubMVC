using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHubDBEntity;

namespace CHubMVC.Models
{
    public class IAModels
    {
        public List<IA_CODE_TYPE> iacode { get; set; }


        public List<IA_PART_AUTOMAP> IaMap { get; set; }

        public string INPUT_PART { get; set; }

        public string PRTNUM { get; set; }


        public List<G_PART_ADDTIONAL> prtaddt { get; set; }
        public string PART_NO { get; set; }



        public List<M_ADRNAM_MST> mam { get; set; }
        public string ADRNAM { get; set; }


        public List<V_IA_TODO_TODAY> iato { get; set; }


    }

}