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
            /// <summary>
            /// --- KPI&DashBoard about CGL China Operation
            /// </summary>
            cgldb,
            /// <summary>
            /// /rp/carmst OPT setup for Carrier mapping with waybill type
            /// </summary>
            rpcar,
            /// <summary>
            /// /rp/adrmst Customized pack setup
            /// </summary>
            cpackset,
            /// <summary>
            /// /rp/index
            /// </summary>
            wbprt,
            /// <summary>
            /// /rp/pack
            /// </summary>
            custpack,
            /// <summary>
            /// /rp/lable
            /// </summary>
            lbprt,
            /// <summary>
            /// /rp/label2
            /// </summary>
            lbprt2,
            /// <summary>
            /// /rp/tracknum
            /// </summary>
            trackinq,
            /// <summary>
            /// /rp/dock
            /// </summary>
            dockdte,
            /// <summary>
            /// /rp/adrcrt
            /// </summary>
            adrcrt,
            /// <summary>
            /// /tc/hscode
            /// </summary>
            hscode,
            /// <summary>
            /// /ia/iacode
            /// </summary>
            iacode,
            /// <summary>
            /// /ia/iamap
            /// </summary>
            iamap,
            /// <summary>
            /// /ia/iascan
            /// </summary>
            iascan,
            /// <summary>
            /// /ia/custaddt
            /// </summary>
            custaddt,
            /// <summary>
            /// /ia/prtaddt
            /// </summary>
            prtaddt,
            /// <summary>
            /// /ia/iascantest
            /// </summary>
            iascantest,
            /// <summary>
            /// /ia/iainq
            /// </summary>
            iainq,
            /// <summary>
            /// /ia/iatoday
            /// </summary>
            iatoday,
            /// <summary>
            /// /order/xcecwb
            /// </summary>
            xcecwb,
            /// <summary>
            /// /order/adrmap
            /// </summary>
            adrmap,
            /// <summary>
            /// /md/mdjvitem
            /// </summary>
            mdjvitem,
            /// <summary>
            /// /md/mdreqinq
            /// </summary>
            mdreqinq,
            /// <summary>
            /// /md/mdreq
            /// </summary>
            mdreq,
            /// <summary>
            /// /md/mdsr
            /// </summary>
            mdsr,
            /// <summary>
            /// /exp/expwb
            /// </summary>
            expwb,
            /// <summary>
            /// /exp/exprate
            /// </summary>
            exprate,
            /// <summary>
            /// /exp/finload
            /// </summary>
            finload,
            /// <summary>
            /// /exp/cinvinq
            /// </summary>
            cinvinq,
            quickscr,
            /// <summary>
            /// /md/mdsupp
            /// </summary>
            mdsupp,
            /// <summary>
            /// /dl/ihubload
            /// </summary>
            ihubload,
            /// <summary>
            /// /ret/retinv
            /// </summary>
            retinv,
            /// <summary>
            /// /ret/retmain
            /// </summary>
            retmain,
            /// <summary>
            /// /adhoc/cwscust
            /// </summary>
            cwscust,
            /// <summary>
            /// /ret/retrestrict
            /// </summary>
            retrestrict,
            /// <summary>
            /// /ds/dsoainq
            /// </summary>
            dsoainq,
            /// <summary>
            /// /ds/dsmain
            /// </summary>
            dsmain,
            /// <summary>
            /// /kpi/kpiset
            /// </summary>
            kpiset,
            /// <summary>
            /// /far/myfar
            /// </summary>
            myfar,
            /// <summary>
            /// /ds/ihubasn
            /// </summary>
            ihubasn,
            quickord,
            /// <summary>
            /// /trc/lbtracec
            /// </summary>
            lbtracec,
            /// <summary>
            /// /trc/lbtracep
            /// </summary>
            lbtracep,
            /// <summary>
            /// /trc/lbtraceinq
            /// </summary>
            lbtraceinq,
            /// <summary>
            /// /mp/mp_custbank
            /// </summary>
            mp_custbank
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
