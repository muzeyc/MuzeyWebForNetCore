using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class MAIL_BASECONFIGDto
    {
        public long? ID { get; set; }
        public string Adresser { get; set; }
        public string AdresserPwd { get; set; }
        public int? PrecautiousTime { get; set; }
        public int? PushedInterval { get; set; }

        public enum DtoEnum
        {
            ID
            ,Adresser
            ,AdresserPwd
            ,PrecautiousTime
            ,PushedInterval
        }

    }
}
