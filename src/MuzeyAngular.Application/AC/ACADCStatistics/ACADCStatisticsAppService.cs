using BusinessLogic;
using CommonUtils;
using System.Data;
using System.Text;

namespace MuzeyServer
{
    public class ACADCStatisticsAppService
    {
        public MuzeyResModel<ACADCStatisticsResDto> GetDatas(MuzeyReqModel<ACADCStatisticsReqDto> reqModel)
        {

            var filter = reqModel.datas[0];
            var sqlStr = GetSql("SE※SE_ANDON");
            var strWhere = MuzeyReqUtil.GetSqlWhere(filter);
            sqlStr += strWhere;
            var resModel = new MuzeyResModel<ACADCStatisticsResDto>();

            var dalLine = new MuzeyBusinessLogic<BASE_LINEDto>("SE※SE_ANDON");
            var dicLine = dalLine.GetDtoDic("", "LineCode");

            var totalCount = 0;
            var dt = SqlHelp.QueryPageList(sqlStr, "MouldNumber", reqModel.offset, reqModel.pageSize, out totalCount).Tables[0];
            resModel.totalCount = totalCount;
            foreach(DataRow dr in dt.Rows)
            {
                var rd = new ACADCStatisticsResDto();
                rd.MouldNumber = dr["MouldNumber"].ToStr();
                rd.LineCodeName = dicLine[dr["LineCode"].ToStr()].LineFullName;
                rd.ADCNum = dr["ADCNum"].ToStr();
                rd.ADCSucScale = dr["ADCSucScale"].ToStr() + "%";
                resModel.datas.Add(rd);
            }
            return resModel;
        }

        private string GetSql(string cs)
        {
            var strBuffer = new StringBuilder();
            strBuffer.AppendLine(string.Format("◎{0}◎", cs));
            strBuffer.AppendLine(" select ");
            strBuffer.AppendLine(" ta.MouldNumber ");
            strBuffer.AppendLine(" ,ta.LineCode ");
            strBuffer.AppendLine(" ,ta.ADCNum ");
            strBuffer.AppendLine(" ,Convert(decimal(18,2),Convert(decimal(18,2),ts.ADCNum) / Convert(decimal(18,2),ta.ADCNum) *100) as ADCSucScale ");
            strBuffer.AppendLine(" from ");
            strBuffer.AppendLine(" (select ");
            strBuffer.AppendLine(" 'all' as quertType ");
            strBuffer.AppendLine(" ,MouldNumber ");
            strBuffer.AppendLine(" ,LineCode ");
            strBuffer.AppendLine(" , COUNT('X') as ADCNum ");
            strBuffer.AppendLine(" from ADC_TIME_STATISTICS t1 ");
            strBuffer.AppendLine(" GROUP BY MouldNumber,LineCode) ta ");
            strBuffer.AppendLine(" INNER JOIN ");
            strBuffer.AppendLine(" (select ");
            strBuffer.AppendLine(" 'suc' as quertType ");
            strBuffer.AppendLine(" ,MouldNumber ");
            strBuffer.AppendLine(" ,LineCode ");
            strBuffer.AppendLine(" , COUNT('X') as ADCNum ");
            strBuffer.AppendLine(" from ADC_TIME_STATISTICS t1 ");
            strBuffer.AppendLine(" where ");
            strBuffer.AppendLine(" t1.AdcResult='1' ");
            strBuffer.AppendLine(" GROUP BY MouldNumber,LineCode) ts ");
            strBuffer.AppendLine(" ON ");
            strBuffer.AppendLine(" ta.MouldNumber = ts.MouldNumber ");
            strBuffer.AppendLine(" AND ta.LineCode = ts.LineCode "); 
            strBuffer.AppendLine(" where 1 = 1 ");
            return strBuffer.ToStr();
        }
    }
}

