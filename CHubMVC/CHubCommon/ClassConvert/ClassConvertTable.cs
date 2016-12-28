using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubCommon
{
    public class ClassConvertTable
    {
        public Dictionary<string, string> AliasAddrSPLConvert = null;
        public Dictionary<string, string> AliasAddrDFLTConvert = null;

        public ClassConvertTable()
        {
            AliasAddrSPLConvert = new Dictionary<string, string>();
            AliasAddrSPLConvert.Add("AliasName", "ALIAS_NAME");
            AliasAddrSPLConvert.Add("Description", "DESCRIPTION");
            AliasAddrSPLConvert.Add("SysID", "SYSID");
            AliasAddrSPLConvert.Add("CustomerNo", "CUSTOMER_NO");
            AliasAddrSPLConvert.Add("Name", "NAME");
            AliasAddrSPLConvert.Add("ActiveInd", "ACTIVEIND");
            AliasAddrSPLConvert.Add("Bill2Location", "BILL_TO_LOCATION");
            AliasAddrSPLConvert.Add("Ship2Location", "SHIP_TO_LOCATION");
            AliasAddrSPLConvert.Add("DestLocation", "DEST_LOCATION");
            AliasAddrSPLConvert.Add("DestName", "DEST_NAME");
            AliasAddrSPLConvert.Add("DestAddr1", "DEST_ADDR_1");
            AliasAddrSPLConvert.Add("DestAddr2", "DEST_ADDR_2");
            AliasAddrSPLConvert.Add("DestAddr3", "DEST_ADDR_3");
            AliasAddrSPLConvert.Add("DestCity", "DEST_CITY");
            AliasAddrSPLConvert.Add("DestContact", "DEST_CONTACT");
            AliasAddrSPLConvert.Add("DestPhone", "DEST_PHONE");
            AliasAddrSPLConvert.Add("DestFax", "DEST_FAX");
            AliasAddrSPLConvert.Add("DestState", "DEST_STATE");
            AliasAddrSPLConvert.Add("DestCountry", "DEST_COUNTRY");
            AliasAddrSPLConvert.Add("DestZip", "DEST_ZIP");
            AliasAddrSPLConvert.Add("RecordDateOSD", "RECORD_DATE_OSD");
            AliasAddrSPLConvert.Add("WareHouse", "WAREHOUSE");
            AliasAddrSPLConvert.Add("DestAttention", "DEST_ATTENTION");
            AliasAddrSPLConvert.Add("LocalDestName", "LOCAL_DEST_NAME");
            AliasAddrSPLConvert.Add("LocalDestAddr1", "LOCAL_DEST_ADDR_1");
            AliasAddrSPLConvert.Add("LocalDestAddr2", "LOCAL_DEST_ADDR_2");
            AliasAddrSPLConvert.Add("LocalDestAddr3", "LOCAL_DEST_ADDR_3");
            AliasAddrSPLConvert.Add("LocalDestCity", "LOCAL_DEST_CITY");
            AliasAddrSPLConvert.Add("LocalDestCountry", "LOCAL_DEST_COUNTRY");
            AliasAddrSPLConvert.Add("LocalDestState", "LOCAL_DEST_STATE");
            AliasAddrSPLConvert.Add("RecordDateOSDL", "RECORD_DATE_OSDL");
            AliasAddrSPLConvert.Add("Days", "DAYS");
            AliasAddrSPLConvert.Add("Distance", "DISTANCE");
            AliasAddrSPLConvert.Add("KGFreight", "KG_FREIGHT");

            AliasAddrDFLTConvert = new Dictionary<string, string>();
            AliasAddrDFLTConvert.Add("AliasName", "ALIAS_NAME");
            AliasAddrDFLTConvert.Add("Description", "DESCRIPTION");
            AliasAddrDFLTConvert.Add("SysID", "SYSID");
            AliasAddrDFLTConvert.Add("CustomerNo", "CUSTOMER_NO");
            AliasAddrDFLTConvert.Add("Name", "NAME");
            AliasAddrDFLTConvert.Add("ActiveInd", "ACTIVEIND");
            AliasAddrDFLTConvert.Add("Bill2Location", "BILL_TO_LOCATION");
            AliasAddrDFLTConvert.Add("Ship2Location", "SHIP_TO_LOCATION");
            AliasAddrDFLTConvert.Add("DestLocation", "");
            AliasAddrDFLTConvert.Add("DestName", "SHIP_TO_NAME");
            AliasAddrDFLTConvert.Add("DestAddr1", "SHIP_TO_ADDR_1");
            AliasAddrDFLTConvert.Add("DestAddr2", "SHIP_TO_ADDR_2");
            AliasAddrDFLTConvert.Add("DestAddr3", "SHIP_TO_ADDR_3");
            AliasAddrDFLTConvert.Add("DestCity", "SHIP_TO_CITY");
            AliasAddrDFLTConvert.Add("DestContact", "SHIP_TO_CONTACT");
            AliasAddrDFLTConvert.Add("DestPhone", "SHIP_TO_PHONE");
            AliasAddrDFLTConvert.Add("DestFax", "SHIP_TO_FAX");
            AliasAddrDFLTConvert.Add("DestState", "SHIP_TO_STATE");
            AliasAddrDFLTConvert.Add("DestCountry", "SHIP_TO_COUNTRY");
            AliasAddrDFLTConvert.Add("DestZip", "SHIP_TO_ZIP");
            AliasAddrDFLTConvert.Add("RecordDateOSD", "RECORD_DATE_OS");
            AliasAddrDFLTConvert.Add("WareHouse", "WAREHOUSE");
            AliasAddrDFLTConvert.Add("DestAttention", "SHIP_TO_ATTEN");
            AliasAddrDFLTConvert.Add("LocalDestName", "LOCAL_SHIP_TO_NAME");
            AliasAddrDFLTConvert.Add("LocalDestAddr1", "LOCAL_SHIP_TO_ADDR_1");
            AliasAddrDFLTConvert.Add("LocalDestAddr2", "LOCAL_SHIP_TO_ADDR_2");
            AliasAddrDFLTConvert.Add("LocalDestAddr3", "LOCAL_SHIP_TO_ADDR_3");
            AliasAddrDFLTConvert.Add("LocalDestCity", "LOCAL_SHIP_TO_CITY");
            AliasAddrDFLTConvert.Add("LocalDestCountry", "LOCAL_SHIP_TO_COUNTRY");
            AliasAddrDFLTConvert.Add("LocalDestState", "LOCAL_SHIP_TO_STATE");
            AliasAddrDFLTConvert.Add("RecordDateOSDL", "RECORD_DATE_OSL");
            AliasAddrDFLTConvert.Add("Days", "DAYS");
            AliasAddrDFLTConvert.Add("Distance", "DISTANCE");
            AliasAddrDFLTConvert.Add("KGFreight", "KG_FREIGHT");
        }

    }
}
