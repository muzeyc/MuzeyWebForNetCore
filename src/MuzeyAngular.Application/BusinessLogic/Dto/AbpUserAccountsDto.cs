using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class AbpUserAccountsDto
    {
        public long ID { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public enum DtoEnum
        {
            ID
            , UserId
            , UserName
        }

    }
}
