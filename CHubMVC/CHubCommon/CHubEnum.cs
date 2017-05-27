using System;
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
            invinq,
            orddft,
            tcmnt,
            tcinq,
            wbentry,
            ittezinq,
            cgldb
        }

        public enum OrderStatusEnum
        {
            /// <summary>
            /// Order Saved
            /// </summary>
            S,
            /// <summary>
            /// Draft
            /// </summary>
            D,
            /// <summary>
            /// Canceled
            /// </summary>
            C
        }

        public enum OrderTypeEnum
        {
            STOCK,
            ECON,
            CUSDWN,
            SCHORD
        }

        public enum PartStatusEnum
        {
            /// <summary>
            /// Active
            /// </summary>
            A,
            /// <summary>
            /// In Active
            /// </summary>
            I
        }

        public enum ShipFromSeqEnum
        {
            Primary = 0,
            Alternative
        }

        public enum ScheduleEnum
        {
            Daily1am,
            Daily2am,
            Daily3am,
            Daily4am,
            Daily5am,
            Daily6am,
            Daily7am,
            Daily8am,
            Daily6pm,
            Daily7pm,
            Daily8pm,
            Daily9pm,
            Daily10pm,
            Daily11pm,
            Daily12am,
            Monday2am,
            Tuesday2am,
            Wednesday2am,
            Thursday2am,
            Friday2am,
            Saturday2am,
            Sunday2am,
            Monday10pm,
            Tuesday10pm,
            Wednesday10pm,
            Thursday10pm,
            Friday10pm,
            Saturday10pm,
            Sunday10pm
        }

    }
}
