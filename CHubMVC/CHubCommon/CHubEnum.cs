﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubCommon
{
    public class CHubEnum
    {
        /// <summary>
        /// Roles Enum
        /// Public for default,
        /// when using, need to be with tolower()
        /// </summary>
        public enum CHubRolesEnum
        {
            Public,
            Admin,
            Csr,
            Read_only,
            Planner,
            Csr_keyuser,
            Transportation
        }

        public enum UserStatesEnum
        {
            A,
            I
        }

        public enum PageNameEnum
        {
            blank,
            qkord,
            syspra,
            usrmnt,
            rolemnt,
            manual,
            ordinq,
            invinq
        }

    }
}
