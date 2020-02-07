using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class RC_CacheDto
    {
        public string CacheCode { get; set; }
        public string Area { get; set; }
        public string Road { get; set; }
        public string Place { get; set; }
        public string VIN { get; set; }
        public string State { get; set; }
        public int? Seq { get; set; }
        public string PlaceState { get; set; }

        public enum DtoEnum
        {
            CacheCode
            ,Area
            ,Road
            ,Place
            ,VIN
            ,State
            ,Seq
            ,PlaceState
        }

    }
}
